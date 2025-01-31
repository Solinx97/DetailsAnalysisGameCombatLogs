using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatAuraService : QueryService<CombatAuraDto, CombatAura>, IMutationService<CombatAuraDto>
{
    private readonly IGenericRepository<CombatAura> _repository;
    private readonly IMapper _mapper;

    public CombatAuraService(IGenericRepository<CombatAura> repository, IMapper mapper) : base(repository, mapper)
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

    public Task<int> UpdateAsync(CombatAuraDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatAuraDto), $"The {nameof(CombatAuraDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatAuraDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatAuraDto), $"The {nameof(CombatAuraDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatAuraDto> CreateInternalAsync(CombatAuraDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatAura>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatAuraDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatAuraDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatAura>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatAuraDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatAura>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(CombatAuraDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto.Name),
                $"The property {nameof(CombatAuraDto.Name)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Creator))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto.Creator),
                $"The property {nameof(CombatAuraDto.Creator)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.Target))
        {
            throw new ArgumentNullException(nameof(CombatAuraDto.Target),
                $"The property {nameof(CombatAuraDto.Target)} of the {nameof(CombatAuraDto)} object can't be null or empty");
        }
    }
}
