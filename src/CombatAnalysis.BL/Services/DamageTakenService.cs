using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageTakenService : QueryService<DamageTakenDto, DamageTaken>, IMutationService<DamageTakenDto>
{
    private readonly IGenericRepository<DamageTaken> _repository;
    private readonly IMapper _mapper;

    public DamageTakenService(IGenericRepository<DamageTaken> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<DamageTakenDto> CreateAsync(DamageTakenDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(DamageTakenDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(DamageTakenDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageTakenDto), $"The {nameof(DamageTakenDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<DamageTakenDto> CreateInternalAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageTakenDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(DamageTakenDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageTaken>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(DamageTakenDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Creator),
                $"The property {nameof(DamageTakenDto.Creator)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Target),
                $"The property {nameof(DamageTakenDto.Target)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageTakenDto.Spell),
                $"The property {nameof(DamageTakenDto.Spell)} of the {nameof(DamageTakenDto)} object can't be null or empty");
        }
    }
}
