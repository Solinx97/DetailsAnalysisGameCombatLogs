using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryService : IService<ResourceRecoveryDto, int>
{
    private readonly IGenericRepository<ResourceRecovery, int> _repository;
    private readonly IMapper _mapper;

    public ResourceRecoveryService(IGenericRepository<ResourceRecovery, int> repository, IMapper mapper)
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

    public Task<int> DeleteAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<ResourceRecoveryDto>>(allData);

        return result;
    }

    public async Task<ResourceRecoveryDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<ResourceRecoveryDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(result);

        return resultMap;
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
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), 
                $"The property {nameof(ResourceRecoveryDto.SpellOrItem)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecovery>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(createdItem);

        return resultMap;
    }

    private async Task<int> DeleteInternalAsync(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), 
                $"The property {nameof(ResourceRecoveryDto.SpellOrItem)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.DeleteAsync(map);

        return rowsAffected;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), 
                $"The property {nameof(ResourceRecoveryDto.SpellOrItem)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
