using AutoMapper;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerDeathService : QueryService<PlayerDeathDto, PlayerDeath>, IMutationService<PlayerDeathDto>
{
    private readonly IGenericRepository<PlayerDeath> _repository;
    private readonly IMapper _mapper;

    public PlayerDeathService(IGenericRepository<PlayerDeath> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PlayerDeathDto> CreateAsync(PlayerDeathDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto), $"The {nameof(PlayerDeathDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(PlayerDeathDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto), $"The {nameof(PlayerDeathDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(PlayerDeathDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto), $"The {nameof(PlayerDeathDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<PlayerDeathDto> CreateInternalAsync(PlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerDeath>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerDeathDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerDeath>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(PlayerDeathDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerDeath>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(PlayerDeathDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(PlayerDeathDto.Username),
                $"The property {nameof(PlayerDeathDto.Username)} of the {nameof(PlayerDeathDto)} object can't be null or empty");
        }
    }
}
