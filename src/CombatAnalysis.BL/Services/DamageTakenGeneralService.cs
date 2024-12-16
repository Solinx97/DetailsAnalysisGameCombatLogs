using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService : IPlayerInfoService<DamageTakenGeneralDto>
{
    private readonly IPlayerInfo<DamageTakenGeneral> _repository;
    private readonly IMapper _mapper;

    public DamageTakenGeneralService(IPlayerInfo<DamageTakenGeneral> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<DamageTakenGeneralDto> CreateAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(allData);

        return result;
    }

    public async Task<DamageTakenGeneralDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetByCombatPlayerIdAsync(int combatPlayerId, int page = 1, int pageSize = 10)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var pagination = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var map = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(pagination);

        return map;
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await _repository.GetByParamAsync(paramName, value);
        var resultMap = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageTakenGeneralDto> CreateInternalAsync(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto),
                $"The property {nameof(DamageTakenGeneralDto.Spell)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto),
                $"The property {nameof(DamageTakenGeneralDto.Spell)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
