using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class ResourceRecoveryGeneralService : IPlayerInfoService<ResourceRecoveryGeneralDto, int>
{
    private readonly ISQLPlayerInfoRepository<ResourceRecoveryGeneral, int> _repository;
    private readonly IMapper _mapper;

    public ResourceRecoveryGeneralService(ISQLPlayerInfoRepository<ResourceRecoveryGeneral, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<ResourceRecoveryGeneralDto> CreateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<ResourceRecoveryGeneralDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<ResourceRecoveryGeneralDto>>(allData);

        return result;
    }

    public async Task<ResourceRecoveryGeneralDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<ResourceRecoveryGeneralDto>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetByCombatPlayerIdAsync(combatPlayerId);
        var resultMap = _mapper.Map<List<ResourceRecoveryGeneralDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<ResourceRecoveryGeneralDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(ResourceRecoveryGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), $"The {nameof(ResourceRecoveryGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<ResourceRecoveryGeneralDto> CreateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), 
                $"The property {nameof(ResourceRecoveryGeneralDto.SpellOrItem)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<ResourceRecoveryGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(ResourceRecoveryGeneralDto item)
    {
        if (string.IsNullOrEmpty(item.SpellOrItem))
        {
            throw new ArgumentNullException(nameof(ResourceRecoveryGeneralDto), 
                $"The property {nameof(ResourceRecoveryGeneralDto.SpellOrItem)} of the {nameof(ResourceRecoveryGeneralDto)} object can't be null or empty");
        }

        var map = _mapper.Map<ResourceRecoveryGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
