using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class DamageDoneGeneralService : QueryService<DamageDoneGeneralDto, DamageDoneGeneral>, IMutationService<DamageDoneGeneralDto>
{
    private readonly IGenericRepository<DamageDoneGeneral> _repository;
    private readonly IMapper _mapper;

    public DamageDoneGeneralService(IGenericRepository<DamageDoneGeneral> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<DamageDoneGeneralDto> CreateAsync(DamageDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(DamageDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneDto), $"The {nameof(DamageDoneGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(DamageDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(DamageDoneGeneralDto), $"The {nameof(DamageDoneGeneralDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<DamageDoneGeneralDto> CreateInternalAsync(DamageDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<DamageDoneGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(DamageDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(DamageDoneGeneralDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<DamageDoneGeneral>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(DamageDoneGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.Spell))
        {
            throw new ArgumentNullException(nameof(DamageDoneGeneralDto.Spell),
                $"The property {nameof(DamageDoneGeneralDto.Spell)} of the {nameof(DamageDoneGeneralDto)} object can't be null or empty");
        }
    }
}
