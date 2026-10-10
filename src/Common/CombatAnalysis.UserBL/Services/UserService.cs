using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;

namespace CombatAnalysis.UserBL.Services;

internal class UserService(IUserRepository repository, IMapper mapper) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AppUserDto?> CreateAsync(AppUserDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));
        ArgumentException.ThrowIfNullOrEmpty(item.FirstName, nameof(item.FirstName));
        ArgumentException.ThrowIfNullOrEmpty(item.LastName, nameof(item.LastName));

        var map = _mapper.Map<AppUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<AppUserDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(string id, AppUserDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentException.ThrowIfNullOrEmpty(item.Username, nameof(item.Username));
        ArgumentException.ThrowIfNullOrEmpty(item.FirstName, nameof(item.FirstName));
        ArgumentException.ThrowIfNullOrEmpty(item.LastName, nameof(item.LastName));

        var map = _mapper.Map<AppUser>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var entityDeleted = await _repository.DeleteAsync(id);
        return entityDeleted;
    }

    public async Task<IEnumerable<AppUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<AppUserDto>>(allData);

        return result;
    }

    public async Task<AppUserDto?> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public async Task<bool> CheckByUsernameAsync(string username)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);

        var users = await GetAllAsync();
        var findByUsername = users.FirstOrDefault(x => x.Username == username);

        return findByUsername != null;
    }

    public async Task<AppUserDto?> GetByIdentityUserIdAsync(string identityUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(identityUserId);

        var result = await _repository.FindByIdentityUserIdAsync(identityUserId);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<AppUserDto>> FindByUsernameStartAtAsync(string startAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(startAt);

        var result = await _repository.FindByUsernameStartAtAsync(startAt);
        var resultMap = _mapper.Map<IEnumerable<AppUserDto>>(result);

        return resultMap;
    }
}
