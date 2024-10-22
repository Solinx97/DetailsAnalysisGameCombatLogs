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
public class GroupChatMessageController : ControllerBase
{
    private readonly IService<GroupChatMessageDto, int> _service;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatMessageController> _logger;

    public GroupChatMessageController(IService<GroupChatMessageDto, int> service, IMapper mapper, ILogger<GroupChatMessageController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        var map = _mapper.Map<IEnumerable<GroupChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        var map = _mapper.Map<GroupChatMessageModel>(result);

        return Ok(map);
    }

    [HttpGet("findByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> Find(int chatId)
    {
        var groupChatMessages = await _service.GetByParamAsync(nameof(GroupChatMessageModel.GroupChatId), chatId);
        return Ok(groupChatMessages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatMessageModel>(result);

            return Ok(resultMap);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _service.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var affectedRows = await _service.DeleteAsync(id);

        return Ok(affectedRows);
    }
}
