using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class HealDoneGeneralService : QueryService<HealDoneGeneralDto, HealDoneGeneral>, IMutationService<HealDoneGeneralDto>
{
    private readonly IGenericRepository<HealDoneGeneral> _repository;
    private readonly IMapper _mapper;

    public HealDoneGeneralService(IGenericRepository<HealDoneGeneral> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<HealDoneGeneralDto> CreateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<HealDoneGeneralDto> CreateInternalAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(HealDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<HealDoneGeneral>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(HealDoneGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto.Spell),
                $"The property {nameof(HealDoneGeneralDto.Spell)} of the {nameof(HealDoneGeneralDto)} object can't be null or empty");
        }
    }
}
