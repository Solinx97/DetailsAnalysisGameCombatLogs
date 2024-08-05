using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class PlayerParseInfoService : IService<PlayerParseInfoDto, int>
{
    private readonly IGenericRepository<PlayerParseInfo, int> _repository;
    private readonly IMapper _mapper;

    public PlayerParseInfoService(IGenericRepository<PlayerParseInfo, int> repository, IMapper mapper)
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

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PlayerParseInfoDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PlayerParseInfoDto>>(allData);

        return result;
    }

    public async Task<PlayerParseInfoDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PlayerParseInfoDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PlayerParseInfoDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PlayerParseInfoDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PlayerParseInfoDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PlayerParseInfoDto), $"The {nameof(PlayerParseInfoDto)} can't be null");
        }

        return UpdateInternalAsync(item);
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
}
