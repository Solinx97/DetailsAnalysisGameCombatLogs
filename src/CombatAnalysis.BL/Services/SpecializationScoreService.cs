using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class SpecializationScoreService : ISpecScoreService
{
    private readonly ISpecScore _repository;
    private readonly IMapper _mapper;

    public SpecializationScoreService(ISpecScore repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<SpecializationScoreDto> CreateAsync(SpecializationScoreDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(SpecializationScoreDto), $"The {nameof(SpecializationScoreDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<SpecializationScoreDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<SpecializationScoreDto>>(allData);

        return result;
    }

    public async Task<SpecializationScoreDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<SpecializationScoreDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<SpecializationScoreDto>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var result = await _repository.GetBySpecIdAsync(specId, bossId, difficult);
        var resultMap = _mapper.Map<IEnumerable<SpecializationScoreDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<SpecializationScoreDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<SpecializationScoreDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(SpecializationScoreDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(SpecializationScoreDto), $"The {nameof(SpecializationScoreDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<SpecializationScoreDto> CreateInternalAsync(SpecializationScoreDto item)
    {
        var map = _mapper.Map<SpecializationScore>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<SpecializationScoreDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(SpecializationScoreDto item)
    {
        var map = _mapper.Map<SpecializationScore>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
