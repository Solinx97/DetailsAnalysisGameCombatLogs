using AutoMapper;
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
    private readonly IChatMessageService<GroupChatMessageDto, int> _chatMessageService;
    private readonly IServiceTransaction<GroupChatUserDto, string> _chatUserService;
    private readonly IService<GroupChatMessageCountDto, int> _chatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatMessageController> _logger;
    private readonly IChatTransactionService _chatTransactionService;

    public GroupChatMessageController(IChatMessageService<GroupChatMessageDto, int> chatMessageService, IService<GroupChatMessageCountDto, int> chatMessageCountService, IServiceTransaction<GroupChatUserDto, string> chatUserService, 
        IMapper mapper, ILogger<GroupChatMessageController> logger, IChatTransactionService chatTransactionService)
    {
        _chatMessageService = chatMessageService;
        _chatMessageCountService = chatMessageCountService;
        _chatUserService = chatUserService;
        _mapper = mapper;
        _logger = logger;
        _chatTransactionService = chatTransactionService;
    }

    [HttpGet("count/{chatId}")]
    public async Task<IActionResult> Count(int chatId)
    {
        var count = await _chatMessageService.CountByChatIdAsync(chatId);

        return Ok(count);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatMessageService.GetAllAsync();
        var map = _mapper.Map<IEnumerable<GroupChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatMessageService.GetByIdAsync(id);
        var map = _mapper.Map<GroupChatMessageModel>(result);

        return Ok(map);
    }

    [HttpGet("getByChatId")]
    public async Task<IActionResult> GetByChatId(int chatId, int pageSize)
    {
        var messages = await _chatMessageService.GetByChatIdAsync(chatId, pageSize);

        return Ok(messages);
    }

    [HttpGet("getMoreByChatId")]
    public async Task<IActionResult> GetMoreByChatId(int chatId, int offset, int pageSize)
    {
        var messages = await _chatMessageService.GetMoreByChatIdAsync(chatId, offset, pageSize);

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatMessageModel chatMessage)
    {
        try
        {
            if (chatMessage == null)
            {
                throw new ArgumentNullException(nameof(chatMessage));
            }

            await _chatTransactionService.BeginTransactionAsync();

            var map = _mapper.Map<GroupChatMessageDto>(chatMessage);
            var createdGroupChatMessage = await _chatMessageService.CreateAsync(map);

            await UpdateMessagesCountAsync(chatMessage.ChatId, chatMessage.GroupChatUserId);

            await _chatTransactionService.CommitTransactionAsync();

            return Ok(createdGroupChatMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", chatMessage);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat Message failed: ${ex.Message}", chatMessage);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<GroupChatMessageDto>(model);
            var result = await _chatMessageService.UpdateAsync(map);

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
        var affectedRows = await _chatMessageService.DeleteAsync(id);

        return Ok(affectedRows);
    }

    private async Task UpdateMessagesCountAsync(int chatId, string meInChatId) 
    {
        var chatUsers = await _chatUserService.GetByParamAsync(nameof(GroupChatUserDto.ChatId), chatId);
        var chatUsersExcludeMe = chatUsers.Where(x => x.Id != meInChatId).ToList();

        var messagesCount = await _chatMessageCountService.GetByParamAsync(nameof(GroupChatMessageCountModel.ChatId), chatId);
        var groupChatMessagesCount = messagesCount.Where(x => chatUsersExcludeMe.Any(y => y.Id == x.GroupChatUserId)).ToList();
        if (groupChatMessagesCount == null)
        {
            throw new ArgumentNullException(nameof(groupChatMessagesCount));
        }

        foreach (var messageCount in groupChatMessagesCount)
        {
            messageCount.Count++;

            await _chatMessageCountService.UpdateAsync(messageCount);
        }
    }
}
