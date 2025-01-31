using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenGeneralService : QueryService<DamageTakenGeneralDto, DamageTakenGeneral>, IMutationService<DamageTakenGeneralDto>
{
    private readonly IGenericRepository<DamageTakenGeneral> _repository;
    private readonly IMapper _mapper;

    public DamageTakenGeneralService(IGenericRepository<DamageTakenGeneral> repository, IMapper mapper) : base(repository, mapper)
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

    public Task<int> UpdateAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(DamageTakenGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto), $"The {nameof(DamageTakenGeneralDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<DamageTakenGeneralDto> CreateInternalAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(DamageTakenGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTakenGeneral>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(DamageTakenGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenGeneralDto.Spell),
                $"The property {nameof(DamageTakenGeneralDto.Spell)} of the {nameof(DamageTakenGeneralDto)} object can't be null or empty");
        }
    }
}
