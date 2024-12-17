using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatService : QueryService<CombatDto, Combat>, IMutationService<CombatDto>
{
    private readonly IGenericRepository<Combat> _repository;
    private readonly IMapper _mapper;

    public CombatService(IGenericRepository<Combat> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatDto> CreateAsync(CombatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(CombatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatDto), $"The {nameof(CombatDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatDto> CreateInternalAsync(CombatDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<Combat>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(CombatDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatDto.Name),
                $"The property {nameof(CombatDto.Name)} of the {nameof(CombatDto)} object can't be null or empty");
        }
        else if (string.IsNullOrEmpty(item.DungeonName))
        {
            throw new ArgumentNullException(nameof(CombatDto.DungeonName),
                $"The property {nameof(CombatDto.DungeonName)} of the {nameof(CombatDto)} object can't be null or empty");
        }
    }
}
