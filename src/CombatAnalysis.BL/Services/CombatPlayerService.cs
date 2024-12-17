using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatPlayerService : QueryService<CombatPlayerDto, CombatPlayer>, IMutationService<CombatPlayerDto>
{
    private readonly IGenericRepository<CombatPlayer> _repository;
    private readonly IMapper _mapper;

    public CombatPlayerService(IGenericRepository<CombatPlayer> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatPlayerDto> CreateAsync(CombatPlayerDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(CombatPlayerDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatPlayerDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatPlayerDto), $"The {nameof(CombatPlayerDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatPlayerDto> CreateInternalAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatPlayerDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatPlayerDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatPlayer>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(CombatPlayerDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CombatPlayerDto.Username),
                $"The property {nameof(CombatPlayerDto.Username)} of the {nameof(CombatPlayerDto)} object can't be null or empty");
        }
    }
}
