using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class BannedUserService : IService<BannedUserDto, int>
{
    private readonly IGenericRepository<BannedUser, int> _repository;
    private readonly IMapper _mapper;

    public BannedUserService(IGenericRepository<BannedUser, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<BannedUserDto> CreateAsync(BannedUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(BannedUserDto), $"The {nameof(BannedUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<BannedUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<BannedUserDto>>(allData);

        return result;
    }

    public async Task<BannedUserDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<BannedUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<BannedUserDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<BannedUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(BannedUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(BannedUserDto), $"The {nameof(BannedUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<BannedUserDto> CreateInternalAsync(BannedUserDto item)
    {
        var map = _mapper.Map<BannedUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<BannedUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(BannedUserDto item)
    {
        var map = _mapper.Map<BannedUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
