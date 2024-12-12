using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryService : IPlayerInfoCountService<ResourceRecoveryDto>
{
    private readonly IPlayerInfoCount<ResourceRecovery> _repository;
    private readonly IMapper _mapper;

    public ResourceRecoveryService(IPlayerInfoCount<ResourceRecovery> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<ResourceRecoveryDto> CreateAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(allData);

        return result;
    }

    public async Task<ResourceRecoveryDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var pagination = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(pagination);

        return map;
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(result);

        return resultMap;
    }

    public async Task<int> CountByCombatPlayerIdAsync(int combatPlayerId)
    {
        var count = await _repository.CountByCombatPlayerIdAsync(combatPlayerId);

        return count;
    }

    public Task<int> UpdateAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<ResourceRecoveryDto> CreateInternalAsync(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto),
                $"The property {nameof(ResourceRecoveryDto.Spell)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecovery>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto),
                $"The property {nameof(ResourceRecoveryDto.Spell)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
