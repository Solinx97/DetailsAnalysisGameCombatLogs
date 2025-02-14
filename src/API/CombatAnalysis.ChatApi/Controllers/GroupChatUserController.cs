﻿using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController : ControllerBase
{
    private readonly IServiceTransaction<GroupChatUserDto, string> _chatUserService;
    private readonly IService<GroupChatMessageCountDto, int> _chatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatUserController> _logger;
    private readonly IChatTransactionService _chatTransactionService;

    public GroupChatUserController(IServiceTransaction<GroupChatUserDto, string> chatUserService, IService<GroupChatMessageCountDto, int> chatMessageCountService, IMapper mapper, 
        ILogger<GroupChatUserController> logger, IChatTransactionService chatTransactionService)
    {
        _chatUserService = chatUserService;
        _chatMessageCountService = chatMessageCountService;
        _mapper = mapper;
        _logger = logger;
        _chatTransactionService = chatTransactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatUserService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _chatUserService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("findUserInChat")]
    public async Task<IActionResult> FindUserInChat(int chatId, string appUserId)
    {
        var result = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), chatId);
        var userInChat = result.FirstOrDefault(x => x.AppUserId == appUserId);

        return Ok(userInChat);
    }

    [HttpGet("findByUserId/{id}")]
    public async Task<IActionResult> FindByUserId(string id)
    {
        var result = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.AppUserId), id);

        return Ok(result);
    }

    [HttpGet("findByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> FindByChatId(int id)
    {
        var result = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), id);

        return Ok(result);
    }

    [HttpGet("findMeInChat")]
    public async Task<IActionResult> FindMeInChat(int chatId, string appUserId)
    {
        var chatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserModel.ChatId), chatId);
        var meInChat = chatUsers.FirstOrDefault(x => x.AppUserId == appUserId);

        return Ok(meInChat);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatUserModel chatUser)
    {
        try
        {
            if (chatUser == null)
            {
                throw new ArgumentNullException(nameof(chatUser));
            }

            await _chatTransactionService.BeginTransactionAsync();

            chatUser.Id = Guid.NewGuid().ToString();

            var map = _mapper.Map<GroupChatUserDto>(chatUser);
            var result = await _chatUserService.CreateAsync(map);

            await CreateChatMessageCountAsync(result.ChatId, result.Id);

            await _chatTransactionService.CommitTransactionAsync();

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat User failed: ${ex.Message}", chatUser);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat User failed: ${ex.Message}", chatUser);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatUserModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatUserDto>(model);
            var result = await _chatUserService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat User failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat User failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var rowsAffected = await _chatUserService.DeleteAsync(id);

        return Ok(rowsAffected);
    }

    private async Task CreateChatMessageCountAsync(int chatId, string groupChatUserId)
    {
        var groupChatMessageCount = new GroupChatMessageCountDto
        {
            ChatId = chatId,
            GroupChatUserId = groupChatUserId
        };

        await _chatMessageCountService.CreateAsync(groupChatMessageCount);
    }
}
