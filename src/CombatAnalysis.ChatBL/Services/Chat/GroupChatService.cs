using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class GroupChatService : IService<GroupChatDto, int>
{
    private readonly IGenericRepository<GroupChat, int> _repository;
    private readonly IMapper _mapper;
    private readonly IChatMessageService<GroupChatMessageDto, int> _groupChatMessageService;
    private readonly IServiceTransaction<GroupChatUserDto, string> _groupChatUserService;
    private readonly IService<GroupChatRulesDto, int> _groupChatRulesService;
    private readonly ILogger<GroupChatService> _logger;

    public GroupChatService(IGenericRepository<GroupChat, int> repository, IMapper mapper, IChatMessageService<GroupChatMessageDto, int> groupChatMessageService,
        IServiceTransaction<GroupChatUserDto, string> groupChatUserService, IService<GroupChatRulesDto, int> groupChatRulesService, ILogger<GroupChatService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _groupChatMessageService = groupChatMessageService;
        _groupChatUserService = groupChatUserService;
        _groupChatRulesService = groupChatRulesService;
        _logger = logger;
    }

    public Task<GroupChatDto> CreateAsync(GroupChatDto item)
    {
        if (item == null)
        {
            _logger.LogError("CreateAsync: GroupChatDto is null");
            throw new ArgumentNullException(nameof(GroupChatDto), $"The {nameof(GroupChatDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await DeleteGroupChatMessagesAsync(id);
            await DeleteGroupChatUsersAsync(id);
            await DeleteGroupChatRulesAsync(id);

            var rowsAffected = await _repository.DeleteAsync(id);

            scope.Complete();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "ArgumentException occurred while deleting group chat with ID: {Id}", id);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Exception occurred while deleting group chat with ID: {Id}", id);

            return 0;
        }
    }

    public async Task<IEnumerable<GroupChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatDto>>(allData);

        return result;
    }

    public async Task<GroupChatDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatDto item)
    {
        if (item == null)
        {
            _logger.LogError("UpdateAsync: GroupChatDto is null");
            
            throw new ArgumentNullException(nameof(GroupChatDto), $"The {nameof(GroupChatDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatDto> CreateInternalAsync(GroupChatDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            _logger.LogError("CreateInternalAsync: GroupChatDto.Name is null or empty");
            
            throw new ArgumentNullException(nameof(GroupChatDto),
                $"The property {nameof(GroupChatDto.Name)} of the {nameof(GroupChatDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            _logger.LogError("UpdateInternalAsync: GroupChatDto.Name is null or empty");
            
            throw new ArgumentNullException(nameof(GroupChatDto),
                $"The property {nameof(GroupChatDto.Name)} of the {nameof(GroupChatDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChat>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteGroupChatMessagesAsync(int chatId)
    {
        var groupChatMessages = await _groupChatMessageService.GetByParamAsync(nameof(GroupChatMessageDto.ChatId), chatId);
        foreach (var item in groupChatMessages)
        {
            var rowsAffected = await _groupChatMessageService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                _logger.LogWarning("Failed to delete message with ID: {UserId} for group chat with ID: {ChatId}", item.Id, chatId);
                
                throw new ArgumentException("Group chat message didn't removed");
            }
        }
    }

    private async Task DeleteGroupChatUsersAsync(int chatId)
    {
        var groupChatUsers = await _groupChatUserService.GetByParamAsync(nameof(GroupChatUserDto.ChatId), chatId);
        foreach (var item in groupChatUsers)
        {
            var rowsAffected = await _groupChatUserService.DeleteUseExistTransactionAsync(item.Id);
            if (rowsAffected == 0)
            {
                _logger.LogWarning("Failed to delete user with ID: {UserId} for group chat with ID: {ChatId}", item.Id, chatId);
                
                throw new ArgumentException("Group chat user didn't removed");
            }
        }
    }

    private async Task DeleteGroupChatRulesAsync(int chatId)
    {
        var groupChatRules = await _groupChatRulesService.GetByParamAsync(nameof(GroupChatRulesDto.ChatId), chatId);
        foreach (var item in groupChatRules)
        {
            var rowsAffected = await _groupChatRulesService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                _logger.LogWarning("Failed to delete rule with ID: {RuleId} for group chat with ID: {ChatId}", item.Id, chatId);
                
                throw new ArgumentException("Group chat rules didn't removed");
            }
        }
    }
}
