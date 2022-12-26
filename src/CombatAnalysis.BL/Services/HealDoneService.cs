using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class HealDoneService : IService<HealDoneDto, int>
{
    private readonly IGenericRepository<HealDone, int> _repository;
    private readonly IMapper _mapper;

    public HealDoneService(IGenericRepository<HealDone, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<HealDoneDto> CreateAsync(HealDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneDto), $"The {nameof(HealDoneDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> DeleteAsync(HealDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneDto), $"The {nameof(HealDoneDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    public async Task<IEnumerable<HealDoneDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<HealDoneDto>>(allData);

        return result;
    }

    public async Task<HealDoneDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<HealDoneDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<HealDoneDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<HealDoneDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(HealDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneDto), $"The {nameof(HealDoneDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<HealDoneDto> CreateInternalAsync(HealDoneDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(HealDoneDto), 
                $"The property {nameof(HealDoneDto.SpellOrItem)} of the {nameof(HealDoneDto)} object can't be null or empty");
        }

        var map = _mapper.Map<HealDone>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneDto>(createdItem);

        return resultMap;
    }

    private async Task<int> DeleteInternalAsync(HealDoneDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(HealDoneDto), 
                $"The property {nameof(HealDoneDto.SpellOrItem)} of the {nameof(HealDoneDto)} object can't be null or empty");
        }

        var map = _mapper.Map<HealDone>(item);
        var rowsAffected = await _repository.DeleteAsync(map);

        return rowsAffected;
    }

    private async Task<int> UpdateInternalAsync(HealDoneDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(HealDoneDto), 
                $"The property {nameof(HealDoneDto.SpellOrItem)} of the {nameof(HealDoneDto)} object can't be null or empty");
        }

        var map = _mapper.Map<HealDone>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
