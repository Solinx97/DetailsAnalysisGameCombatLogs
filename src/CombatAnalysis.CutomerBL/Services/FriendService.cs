using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class FriendService : IService<FriendDto, int>
{
    private readonly IGenericRepository<Friend, int> _repository;
    private readonly IMapper _mapper;

    public FriendService(IGenericRepository<Friend, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<FriendDto> CreateAsync(FriendDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(FriendDto), $"The {nameof(FriendDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<FriendDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<FriendDto>>(allData);

        return result;
    }

    public async Task<FriendDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<FriendDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<FriendDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<FriendDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(FriendDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(FriendDto), $"The {nameof(FriendDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<FriendDto> CreateInternalAsync(FriendDto item)
    {
        var map = _mapper.Map<Friend>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<FriendDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(FriendDto item)
    {
        var map = _mapper.Map<Friend>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
