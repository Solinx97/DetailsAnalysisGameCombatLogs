using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services;

internal class HealDoneGeneralService : IService<HealDoneGeneralDto, int>
{
    private readonly IGenericRepository<HealDoneGeneral, int> _repository;
    private readonly IMapper _mapper;

    public HealDoneGeneralService(IGenericRepository<HealDoneGeneral, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<HealDoneGeneralDto> CreateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<HealDoneGeneralDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<HealDoneGeneralDto>>(allData);

        return result;
    }

    public async Task<HealDoneGeneralDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<HealDoneGeneralDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<HealDoneGeneralDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(HealDoneGeneralDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(HealDoneGeneralDto), $"The {nameof(HealDoneGeneralDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<HealDoneGeneralDto> CreateInternalAsync(HealDoneGeneralDto item)
    {
        var map = _mapper.Map<HealDoneGeneral>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<HealDoneGeneralDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(HealDoneGeneralDto item)
    {
        var map = _mapper.Map<HealDoneGeneral>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
