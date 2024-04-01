using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Chat;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Chat;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.CommunicationBL.Services.Chat;

internal class GroupChatUserService : IServiceTransaction<GroupChatUserDto, string>
{
    private readonly IGenericRepository<GroupChatUser, string> _repository;
    private readonly IService<UnreadGroupChatMessageDto, int> _unreadGroupChatMessageService;
    private readonly IService<GroupChatMessageCountDto, int> _groupChatMessageCountService;
    private readonly IMapper _mapper;
    private readonly ISqlContextService _sqlContextService;

    public GroupChatUserService(IGenericRepository<GroupChatUser, string> repository, IService<UnreadGroupChatMessageDto, int> unreadGroupChatMessageService,
        IService<GroupChatMessageCountDto, int> groupChatMessageCountService, IMapper mapper,
        ISqlContextService sqlContextService)
    {
        _repository = repository;
        _unreadGroupChatMessageService = unreadGroupChatMessageService;
        _groupChatMessageCountService = groupChatMessageCountService;
        _mapper = mapper;
        _sqlContextService = sqlContextService;
    }

    public Task<GroupChatUserDto> CreateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync(false);
        try
        {
            await DeleteUnreadGroupChatMessageAsync(id);
            await DeleteGroupChatMessageCountAsync(id);

            transaction.CreateSavepoint("BeforeDeleteGroupChatUser");

            var rowsAffected = await _repository.DeleteAsync(id);

            await transaction.CommitAsync();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteGroupChatUser");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteGroupChatUser");

            return 0;
        }
    }

    public async Task<int> DeleteUseExistTransactionAsync(IDbContextTransaction transaction, string id)
    {
        try
        {
            await DeleteUnreadGroupChatMessageAsync(id);
            await DeleteGroupChatMessageCountAsync(id);

            transaction.CreateSavepoint("BeforeDeleteGroupChatUser");

            var rowsAffected = await _repository.DeleteAsync(id);

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteGroupChatUser");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteGroupChatUser");

            return 0;
        }
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatUserDto>>(allData);

        return result;
    }

    public async Task<GroupChatUserDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatUserDto> CreateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteUnreadGroupChatMessageAsync(string userId)
    {
        var unreadGroupChatMessage = await _unreadGroupChatMessageService.GetByParamAsync(nameof(UnreadGroupChatMessageDto.GroupChatUserId), userId);
        foreach (var item in unreadGroupChatMessage)
        {
            var rowsAffected = await _unreadGroupChatMessageService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Unread group chat message didn't removed");
            }
        }
    }

    private async Task DeleteGroupChatMessageCountAsync(string customerId)
    {
        var groupChatMessageCount = await _groupChatMessageCountService.GetByParamAsync(nameof(GroupChatMessageCountDto.GroupChatUserId), customerId);
        if (!groupChatMessageCount.Any())
        {
            return;
        }

        var rowsAffected = await _groupChatMessageCountService.DeleteAsync(groupChatMessageCount.FirstOrDefault().Id);
        if (rowsAffected == 0)
        {
            throw new ArgumentException("Unread group chat message didn't removed");
        }
    }
}
