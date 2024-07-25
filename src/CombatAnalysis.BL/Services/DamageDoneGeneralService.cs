using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneGeneralService : IPlayerInfoService<DamageDoneGeneralDto, int>
{
    private readonly ISQLPlayerInfoRepository<DamageDoneGeneral, int> _repository;
    private readonly IMapper _mapper;

    public DamageDoneGeneralService(ISQLPlayerInfoRepository<DamageDoneGeneral, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<DamageDoneGeneralDto> CreateAsync(DamageDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<DamageDoneGeneralDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<DamageDoneGeneralDto>>(allData);

        return result;
    }

    public async Task<DamageDoneGeneralDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<DamageDoneGeneralDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageDoneGeneralDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<List<DamageDoneGeneralDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageDoneGeneralDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(DamageDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageDoneGeneralDto> CreateInternalAsync(DamageDoneGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageDoneGeneralDto), 
                $"The property {nameof(DamageDoneGeneralDto.SpellOrItem)} of the {nameof(DamageDoneGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageDoneGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageDoneGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageDoneGeneralDto), 
                $"The property {nameof(DamageDoneGeneralDto.SpellOrItem)} of the {nameof(DamageDoneGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
