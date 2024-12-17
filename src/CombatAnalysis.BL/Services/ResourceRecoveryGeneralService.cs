using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService : QueryService<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>, IMutationService<ResourceRecoveryGeneralDto>
{
    private readonly IGenericRepository<ResourceRecoveryGeneral> _repository;
    private readonly IMapper _mapper;

    public ResourceRecoveryGeneralService(IGenericRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<ResourceRecoveryGeneralDto> CreateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<ResourceRecoveryGeneralDto> CreateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(ResourceRecoveryGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto.Spell),
                $"The property {nameof(ResourceRecoveryGeneralDto.Spell)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }
    }
}
