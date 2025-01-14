using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Models.Containers;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatController : ControllerBase
{
    private readonly IService<GroupChatDto, int> _chatService;
    private readonly IService<GroupChatRulesDto, int> _chatRulesService;
    private readonly IServiceTransaction<GroupChatUserDto, string> _chatUserService;
    private readonly IService<GroupChatMessageCountDto, int> _chatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ILogger<GroupChatController> _logger;
    private readonly IChatTransactionService _chatTransactionService;

    public GroupChatController(IService<GroupChatDto, int> chatService, IService<GroupChatMessageCountDto, int> chatMessageCountService, IService<GroupChatRulesDto, int> chatRulesService,
        IServiceTransaction<GroupChatUserDto, string> chatUserService, IMapper mapper, IChatTransactionService chatTransactionService,
        ILogger<GroupChatController> logger)
    {
        _chatService = chatService;
        _chatMessageCountService = chatMessageCountService;
        _chatRulesService = chatRulesService;
        _chatUserService = chatUserService;
        _mapper = mapper;
        _chatTransactionService = chatTransactionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _chatService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chatService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupChatContainerModel container)
    {
        try
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            await _chatTransactionService.BeginTransactionAsync();

            var chatMap = _mapper.Map<GroupChatDto>(container.GroupChat);
            var result = await _chatService.CreateAsync(chatMap);

            await CreateChatUserAsync(result.Id, container.GroupChatRules, container.GroupChatUser);

            await _chatTransactionService.CommitTransactionAsync();

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Create Group Chat failed: ${ex.Message}", container);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Create Group Chat failed: ${ex.Message}", container);

            await _chatTransactionService.RollbackTransactionAsync();

            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(GroupChatModel chat)
    {
        try
        {
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            var chatMap = _mapper.Map<GroupChatDto>(chat);
            var result = await _chatService.UpdateAsync(chatMap);

            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Update Group Chat failed: ${ex.Message}", chat);

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Update Group Chat failed: ${ex.Message}", chat);

            return BadRequest();
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _chatService.DeleteAsync(id);

        return Ok(result);
    }

    private async Task CreateChatUserAsync(int chatId, GroupChatRulesModel chatRules, GroupChatUserModel chatUser)
    {
        chatRules.ChatId = chatId;

        var chatRulesMap = _mapper.Map<GroupChatRulesDto>(chatRules);
        await _chatRulesService.CreateAsync(chatRulesMap);

        chatUser.ChatId = chatId;

        var chatUserMap = _mapper.Map<GroupChatUserDto>(chatUser);
        var result = await _chatUserService.CreateAsync(chatUserMap);

        var messageCount = new GroupChatMessageCountDto
        {
            ChatId = chatId,
            GroupChatUserId = result.Id,
            Count = 0
        };

        await _chatMessageCountService.CreateAsync(messageCount);
    }
}
