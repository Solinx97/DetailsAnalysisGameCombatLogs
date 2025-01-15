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
public class PersonalChatMessageController : ControllerBase
{
    private readonly IChatMessageService<PersonalChatMessageDto, int> _chatMessageService;
    private readonly IService<PersonalChatMessageCountDto, int> _chatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonalChatMessageController> _logger;
    private readonly IChatTransactionService _chatTransactionService;

    public PersonalChatMessageController(IChatMessageService<PersonalChatMessageDto, int> chatMessageService, IService<PersonalChatMessageCountDto, int> chatMessageCountService, IMapper mapper, 
        ILogger<PersonalChatMessageController> logger, IChatTransactionService chatTransactionService)
    {
        _chatMessageService = chatMessageService;
        _chatMessageCountService = chatMessageCountService;
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
        var map = _mapper.Map<IEnumerable<PersonalChatMessageModel>>(result);

        return Ok(map);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatMessageService.GetByIdAsync(id);
        var map = _mapper.Map<PersonalChatMessageModel>(result);

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
    public async Task<IActionResult> Create(PersonalChatMessageModel chatMessage)
    {
        try
        {
            if (chatMessage == null)
            {
                throw new ArgumentNullException(nameof(chatMessage));
            }

            await _chatTransactionService.BeginTransactionAsync();

            var map = _mapper.Map<PersonalChatMessageDto>(chatMessage);
            var result = await _chatMessageService.CreateAsync(map);

            var messagesCount = await _chatMessageCountService.GetAllAsync();
            var targetMessageCount = messagesCount.Where(x => x.ChatId == result.ChatId && x.AppUserId == result.AppUserId).FirstOrDefault();
            if (targetMessageCount == null)
            {
                throw new ArgumentNullException(nameof(targetMessageCount));
            }

            targetMessageCount.Count++;

            await _chatMessageCountService.UpdateAsync(targetMessageCount);

            await _chatTransactionService.CommitTransactionAsync();

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message failed: ${ex.Message}", chatMessage);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Personal Chat Message failed: ${ex.Message}", chatMessage);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonalChatMessageModel model)
    {
        try
        {
            var map = _mapper.Map<PersonalChatMessageDto>(model);
            var result = await _chatMessageService.UpdateAsync(map);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Personal Chat Message failed: ${ex.Message}", model);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rowsAffected = await _chatMessageService.DeleteAsync(id);

        return Ok(rowsAffected);
    }
}
