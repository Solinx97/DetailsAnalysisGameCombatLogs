using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService : IService<DamageTakenGeneralDto, int>
{
    private readonly IGenericRepository<DamageTakenGeneral, int> _repository;
    private readonly IMapper _mapper;

    public DamageTakenGeneralService(IGenericRepository<DamageTakenGeneral, int> repository, IMapper mapper)
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

    public Task<int> DeleteAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<DamageTakenGeneralDto>>(allData);

        return result;
    }

    public async Task<DamageTakenGeneralDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<DamageTakenGeneralDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
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
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), 
                $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), 
                $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.DeleteAsync(map);

        return rowsAffected;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto),
                $"The property {nameof(DamageTakenGeneralDto.SpellOrItem)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
