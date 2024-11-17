using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerPositionService : IPlayerInfoService<CombatPlayerPositionDto, int>
{
    private readonly IPlayerInfo<CombatPlayerPosition, int> _repository;
    private readonly IMapper _mapper;

    public CombatPlayerPositionService(IPlayerInfo<CombatPlayerPosition, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatPlayerPositionDto> CreateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CombatPlayerPositionDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CombatPlayerPositionDto>>(allData);

        return result;
    }

    public async Task<CombatPlayerPositionDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CombatPlayerPositionDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CombatPlayerPositionDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<CombatPlayerPositionDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CombatPlayerPositionDto>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId, page, pageSize);
        var map = _mapper.Map<IEnumerable<CombatPlayerPositionDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CombatPlayerPositionDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CombatPlayerPositionDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CombatPlayerPositionDto> CreateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerPositionDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
