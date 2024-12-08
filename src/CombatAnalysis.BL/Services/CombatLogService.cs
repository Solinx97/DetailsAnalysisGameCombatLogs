using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatLogService : IService<CombatLogDto>
{
    private readonly IGenericRepository<CombatLog> _repository;
    private readonly IMapper _mapper;

    public CombatLogService(IGenericRepository<CombatLog> userRepository, IMapper mapper)
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

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CombatLogDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CombatLogDto>>(allData);

        return result;
    }

    public async Task<CombatLogDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CombatLogDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CombatLogDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CombatLogDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CombatLogDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogDto), $"The {nameof(CombatLogDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CombatLogDto> CreateInternalAsync(CombatLogDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatLogDto),
                $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CombatLog>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatLogDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatLogDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CombatLogDto),
                $"The property {nameof(CombatLogDto.Name)} of the {nameof(CombatLogDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CombatLog>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
