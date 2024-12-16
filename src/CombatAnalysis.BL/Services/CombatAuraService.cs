using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatAuraService : IService<CombatAuraDto>
{
    private readonly IGenericRepository<CombatAura> _repository;
    private readonly IMapper _mapper;

    public CombatAuraService(IGenericRepository<CombatAura> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatAuraDto> CreateAsync(CombatAuraDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatAuraDto), $"The {nameof(CombatAuraDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CombatAuraDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CombatAuraDto>>(allData);

        return result;
    }

    public async Task<CombatAuraDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CombatAuraDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CombatAuraDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await _repository.GetByParamAsync(paramName, value);
        var resultMap = _mapper.Map<IEnumerable<CombatAuraDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CombatAuraDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatAuraDto), $"The {nameof(CombatAuraDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CombatAuraDto> CreateInternalAsync(CombatAuraDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Name)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Creator)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Target)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CombatAura>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatAuraDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatAuraDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Name)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Creator)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto),
                $"The property {nameof(CombatAuraDto.Target)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CombatAura>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
