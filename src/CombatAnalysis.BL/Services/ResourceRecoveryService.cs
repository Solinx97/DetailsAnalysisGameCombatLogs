using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryService : QueryService<ResourceRecoveryDto, ResourceRecovery>, IMutationService<ResourceRecoveryDto>
{
    private readonly IGenericRepository<ResourceRecovery> _repository;
    private readonly IMapper _mapper;

    public ResourceRecoveryService(IGenericRepository<ResourceRecovery> repository, IMapper mapper) : base(repository, mapper)
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

    public Task<int> UpdateAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(ResourceRecoveryDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto), $"The {nameof(ResourceRecoveryDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<ResourceRecoveryDto> CreateInternalAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(ResourceRecoveryDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecovery>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(ResourceRecoveryDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Creator),
                $"The property {nameof(ResourceRecoveryDto.Creator)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Target),
                $"The property {nameof(ResourceRecoveryDto.Target)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryDto.Spell),
                $"The property {nameof(ResourceRecoveryDto.Spell)} of the {nameof(ResourceRecoveryDto)} object can't be null or empty");
        }
    }
}
