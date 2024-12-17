using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class CombatLogByUserService : QueryService<CombatLogByUserDto, CombatLogByUser>, IMutationService<CombatLogByUserDto>
{
    private readonly IGenericRepository<CombatLogByUser> _repository;
    private readonly IMapper _mapper;

    public CombatLogByUserService(IGenericRepository<CombatLogByUser> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CombatLogByUserDto> CreateAsync(CombatLogByUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogByUserDto), $"The {nameof(CombatLogByUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(CombatLogByUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogByUserDto), $"The {nameof(CombatLogByUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(CombatLogByUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogByUserDto), $"The {nameof(CombatLogByUserDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<CombatLogByUserDto> CreateInternalAsync(CombatLogByUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLogByUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatLogByUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatLogByUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLogByUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(CombatLogByUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<CombatLogByUser>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(CombatLogByUserDto item)
    {
        if (item.CombatsInQueue < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(CombatLogByUserDto.CombatsInQueue),
                $"The property {nameof(CombatLogByUserDto.CombatsInQueue)} of the {nameof(CombatLogByUserDto)} should be positive");
        }
    }
}
