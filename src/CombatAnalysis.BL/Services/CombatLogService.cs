using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatLogService : QueryService<CombatLogDto, CombatLog>, IMutationService<CombatLogDto>
{
    private readonly IGenericRepository<CombatLog> _repository;
    private readonly IMapper _mapper;

    public CombatLogService(IGenericRepository<CombatLog> userRepository, IMapper mapper) : base(userRepository, mapper)
    {
        _repository = userRepository;
        _mapper = mapper;
    }

    public Task<CombatLogDto> CreateAsync(CombatLogDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(CombatLogDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatLogDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatLogDto> CreateInternalAsync(CombatLogDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLog>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatLogDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatLogDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLog>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatLogDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLog>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(CombatLogDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatLogDto.Name),
                $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
        }
    }
}
