using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Chat;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Chat;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Chat;

internal class GroupChatMessageCountService : IService<GroupChatMessageCountDto, int>
{
    private readonly IGenericRepository<GroupChatMessageCount, int> _repository;
    private readonly IMapper _mapper;

    public GroupChatMessageCountService(IGenericRepository<GroupChatMessageCount, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<GroupChatMessageCountDto> CreateAsync(GroupChatMessageCountDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageCountDto), $"The {nameof(GroupChatMessageCountDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<GroupChatMessageCountDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatMessageCountDto>>(allData);

        return result;
    }

    public async Task<GroupChatMessageCountDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatMessageCountDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatMessageCountDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatMessageCountDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatMessageCountDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageCountDto), $"The {nameof(GroupChatMessageCountDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatMessageCountDto> CreateInternalAsync(GroupChatMessageCountDto item)
    {
        var map = _mapper.Map<GroupChatMessageCount>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatMessageCountDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatMessageCountDto item)
    {
        var map = _mapper.Map<GroupChatMessageCount>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
