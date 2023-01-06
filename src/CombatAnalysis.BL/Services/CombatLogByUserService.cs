using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class CombatLogByUserService : IService<CombatLogByUserDto, int>
{
    private readonly IGenericRepository<CombatLogByUser, int> _repository;
    private readonly IMapper _mapper;

    public CombatLogByUserService(IGenericRepository<CombatLogByUser, int> userRepository, IMapper mapper)
    {
        _repository = userRepository;
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

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CombatLogByUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CombatLogByUserDto>>(allData);

        return result;
    }

    public async Task<CombatLogByUserDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CombatLogByUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CombatLogByUserDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CombatLogByUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CombatLogByUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CombatLogByUserDto), $"The {nameof(CombatLogByUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CombatLogByUserDto> CreateInternalAsync(CombatLogByUserDto item)
    {
        var map = _mapper.Map<CombatLogByUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CombatLogByUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CombatLogByUserDto item)
    {
        var map = _mapper.Map<CombatLogByUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
