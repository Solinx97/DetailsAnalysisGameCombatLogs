using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.BL.Services.General;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services;

internal class PlayerParseInfoService : QueryService<PlayerParseInfoDto, PlayerParseInfo>, IMutationService<PlayerParseInfoDto>
{
    private readonly IGenericRepository<PlayerParseInfo> _repository;
    private readonly IMapper _mapper;

    public PlayerParseInfoService(IGenericRepository<PlayerParseInfo> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PlayerParseInfoDto> CreateAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> UpdateAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    public Task<int> DeleteAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    private async Task<PlayerParseInfoDto> CreateInternalAsync(PlayerParseInfoDto item)
    {
        var map = _mapper.Map<PlayerParseInfo>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PlayerParseInfoDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PlayerParseInfoDto item)
    {
        var map = _mapper.Map<PlayerParseInfo>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task<int> DeleteInternalAsync(PlayerParseInfoDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<PlayerParseInfo>(item);
        var affectedRows = await _repository.DeleteAsync(map);

        return affectedRows;
    }

    private void CheckParams(PlayerParseInfoDto item)
    {
        if (item.Difficult < 0)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto.Difficult),
                $"The property {nameof(PlayerParseInfoDto.Difficult)} of the {nameof(PlayerParseInfoDto)} should be positive");
        }
    }
}
