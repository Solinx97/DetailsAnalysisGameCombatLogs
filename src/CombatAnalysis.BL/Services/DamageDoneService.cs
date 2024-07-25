using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneService : IPlayerInfoService<DamageDoneDto, int>
{
    private readonly ISQLPlayerInfoRepository<DamageDone, int> _repository;
    private readonly IMapper _mapper;

    public DamageDoneService(ISQLPlayerInfoRepository<DamageDone, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<DamageDoneDto> CreateAsync(DamageDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<DamageDoneDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<DamageDoneDto>>(allData);

        return result;
    }

    public async Task<DamageDoneDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<DamageDoneDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageDoneDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<List<DamageDoneDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageDoneDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<DamageDoneDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(DamageDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<DamageDoneDto> CreateInternalAsync(DamageDoneDto item)
    {
        if (string.IsNullOrEmpty(item.FromPlayer))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto),
                $"The property {nameof(DamageDoneDto.FromPlayer)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.ToEnemy))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), 
                $"The property {nameof(DamageDoneDto.ToEnemy)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), 
                $"The property {nameof(DamageDoneDto.SpellOrItem)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageDone>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageDoneDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageDoneDto item)
    {
        if (string.IsNullOrEmpty(item.FromPlayer))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), 
                $"The property {nameof(DamageDoneDto.FromPlayer)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.ToEnemy))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), 
                $"The property {nameof(DamageDoneDto.ToEnemy)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), 
                $"The property {nameof(DamageDoneDto.SpellOrItem)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageDone>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
