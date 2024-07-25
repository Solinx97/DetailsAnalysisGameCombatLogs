using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class GroupChatRulesService : IService<GroupChatRulesDto, int>
{
    private readonly IGenericRepository<GroupChatRules, int> _repository;
    private readonly IMapper _mapper;

    public GroupChatRulesService(IGenericRepository<GroupChatRules, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<GroupChatRulesDto> CreateAsync(GroupChatRulesDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatRulesDto), $"The {nameof(GroupChatRulesDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<GroupChatRulesDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatRulesDto>>(allData);

        return result;
    }

    public async Task<GroupChatRulesDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatRulesDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatRulesDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatRulesDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatRulesDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatRulesDto), $"The {nameof(GroupChatRulesDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatRulesDto> CreateInternalAsync(GroupChatRulesDto item)
    {
        var map = _mapper.Map<GroupChatRules>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatRulesDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatRulesDto item)
    {
        var map = _mapper.Map<GroupChatRules>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
