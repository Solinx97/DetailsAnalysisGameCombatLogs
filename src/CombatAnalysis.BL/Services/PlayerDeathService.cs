using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class PlayerDeathService : IPlayerInfoService<PlayerDeathDto>
{
    private readonly IPlayerInfo<PlayerDeath> _repository;
    private readonly IMapper _mapper;

    public PlayerDeathService(IPlayerInfo<PlayerDeath> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PlayerDeathDto> CreateAsync(PlayerDeathDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto), $"The {nameof(PlayerDeathDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PlayerDeathDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PlayerDeathDto>>(allData);

        return result;
    }

    public async Task<PlayerDeathDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PlayerDeathDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PlayerDeathDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<PlayerDeathDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PlayerDeathDto>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var pagination = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var map = _mapper.Map<IEnumerable<PlayerDeathDto>>(pagination);

        return map;
    }

    public async Task<IEnumerable<PlayerDeathDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PlayerDeathDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PlayerDeathDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto), $"The {nameof(PlayerDeathDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<PlayerDeathDto> CreateInternalAsync(PlayerDeathDto item)
    {
        var map = _mapper.Map<PlayerDeath>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerDeathDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PlayerDeathDto item)
    {
        var map = _mapper.Map<PlayerDeath>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
