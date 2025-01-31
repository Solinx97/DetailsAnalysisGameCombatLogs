using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostDislikeService : IService<UserPostDislikeDto, int>
{
    private readonly IGenericRepository<UserPostDislike, int> _repository;
    private readonly IMapper _mapper;

    public UserPostDislikeService(IGenericRepository<UserPostDislike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<UserPostDislikeDto> CreateAsync(UserPostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostDislikeDto), $"The {nameof(UserPostDislikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<UserPostDislikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostDislikeDto>>(allData);

        return result;
    }

    public async Task<UserPostDislikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostDislikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDislikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<UserPostDislikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(UserPostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostDislikeDto), $"The {nameof(UserPostDislikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<UserPostDislikeDto> CreateInternalAsync(UserPostDislikeDto item)
    {
        var map = _mapper.Map<UserPostDislike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostDislikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UserPostDislikeDto item)
    {
        var map = _mapper.Map<UserPostDislike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
