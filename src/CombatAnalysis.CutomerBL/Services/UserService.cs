using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services.User;

internal class UserService : IUserService<AppUserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<AppUserDto> CreateAsync(AppUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> DeleteAsync(AppUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    public async Task<IEnumerable<AppUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<AppUserDto>>(allData);

        return result;
    }

    public async Task<AppUserDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public async Task<bool> CheckByUsernameAsync(string username)
    {
        var users = await GetAllAsync();
        var findByUsername = users.FirstOrDefault(x => x.Username == username);

        return findByUsername != null;
    }

    public async Task<AppUserDto> GetAsync(string identityUserId)
    {
        var result = await _repository.GetAsync(identityUserId);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(AppUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(AppUserDto), $"The {nameof(AppUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<AppUserDto> CreateInternalAsync(AppUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.Username)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.FirstName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.FirstName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.LastName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.LastName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<AppUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<AppUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> DeleteInternalAsync(AppUserDto item)
    {
        var map = _mapper.Map<AppUser>(item);
        var rowsAffected = await _repository.DeleteAsync(map);

        return rowsAffected;
    }

    private async Task<int> UpdateInternalAsync(AppUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.Username)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.FirstName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.FirstName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.LastName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.LastName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<AppUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
