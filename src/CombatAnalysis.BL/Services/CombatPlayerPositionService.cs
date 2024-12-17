using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerPositionService : QueryService<CombatPlayerPositionDto, CombatPlayerPosition>, IMutationService<CombatPlayerPositionDto>
{
    private readonly IGenericRepository<CombatPlayerPosition> _repository;
    private readonly IMapper _mapper;

    public CombatPlayerPositionService(IGenericRepository<CombatPlayerPosition> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatPlayerPositionDto> CreateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatPlayerPositionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerPositionDto), $"The {nameof(CombatPlayerPositionDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatPlayerPositionDto> CreateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerPositionDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatPlayerPositionDto item)
    {
        var map = _mapper.Map<CombatPlayerPosition>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }
}
