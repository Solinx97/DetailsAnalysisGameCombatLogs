using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneService : QueryService<HealDoneDto, HealDone>, IMutationService<HealDoneDto>
{
    private readonly IGenericRepository<HealDone> _repository;
    private readonly IMapper _mapper;

    public HealDoneService(IGenericRepository<HealDone> repository, IMapper mapper) : base(repository, mapper)
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

    public Task<int> UpdateAsync(HealDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneDto), $"The {nameof(HealDoneDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(HealDoneDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneDto), $"The {nameof(HealDoneDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<HealDoneDto> CreateInternalAsync(HealDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDone>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(HealDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDone>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(HealDoneDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDone>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(HealDoneDto item)
    {
        if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Creator),
                $"The property {nameof(DamageDoneDto.Creator)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Target),
                $"The property {nameof(DamageDoneDto.Target)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageDoneDto.Spell),
                $"The property {nameof(DamageDoneDto.Spell)} of the {nameof(DamageDoneDto)} object can't be null or empty");
        }
    }
}
