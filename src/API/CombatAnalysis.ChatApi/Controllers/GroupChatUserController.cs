using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatAPI.Core;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Patches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController(IGroupChatUserService chatUserService, IMapper mapper, ILogger<GroupChatUserController> logger) : ControllerBase
{
    private readonly IGroupChatUserService _chatUserService = chatUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupChatUserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var groupChatUsers = await _chatUserService.GetAllAsync();

        return Ok(groupChatUsers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var groupChatUser = await _chatUserService.GetByIdAsync(id);

            return Ok(groupChatUser);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Get group chat user {Id} failed: Group chat user not found.", ex.UserId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get group chat user {Id} failed. Something wrong during extracting group chat user.", id);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpGet("findByAppUserId")]
    public async Task<IActionResult> FindByAppUserId([Required] [Range(1, int.MaxValue)] int chatId, [Required] Guid appUserId)
    {
        try
        {
            var groupChatUser = await _chatUserService.FindByAppUserIdAsync(chatId, appUserId);

            return Ok(groupChatUser);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogInformation("Get group chat user by user {Id} failed. Group chat user not found.", appUserId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Get group chat user by user {Id} failed. Something wrong during extracting group chat user.", appUserId);

            return this.ExtractDomainCode(ex.Code);
        }
    }

    [HttpGet("findAllByAppUserId/{appUserId}")]
    public async Task<IActionResult> FindAllByAppUserId(Guid appUserId)
    {
        var groupChatUsers = await _chatUserService.FindAllByAppUserIdAsync(appUserId);

        return Ok(groupChatUsers);
    }

    [HttpGet("findAll/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindAll(int chatId)
    {
        var groupChatUsers = await _chatUserService.FindAllAsync(chatId);

        return Ok(groupChatUsers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatUserModel chatUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatUser create received: {@ChatUser}", chatUser);

                return ValidationProblem(ModelState);
            }

            var map = _mapper.Map<GroupChatUserDto>(chatUser);
            var createdGroupChatUser = await _chatUserService.CreateAsync(map);

            return Ok(createdGroupChatUser);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat user.");

            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] GroupChatUserPatch groupChatUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid GroupChatUser update request received: {@GroupChatUser}", groupChatUser);

                return ValidationProblem(ModelState);
            }

            if (id != groupChatUser.Id)
            {
                return BadRequest("Route ID and body ID do not match.");
            }

            await _chatUserService.UpdateChatUserAsync(groupChatUser.Id, groupChatUser.LastReadMessageId, groupChatUser.UnreadMessages);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update group chat user {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Update group chat user {Id} failed. Something wrong during updating group chat user.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _chatUserService.DeleteAsync(id);

            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Delete group chat user {Id} failed. Entity '{Entity}' ({EntityId}) not found.", id, nameof(ex.EntityType), ex.EntityId);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "Delete group chat user {Id} failed. Something wrong during deleting group chat user.", id);

            return this.ExtractDomainCode(ex.Code);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "The resource was modified by another user. Please refresh and try again.");

            return Conflict(new { message = "The resource was modified by another user. Please refresh and try again." });
        }
    }
}
