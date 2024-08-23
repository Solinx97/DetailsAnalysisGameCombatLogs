using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostLikeService : IService<UserPostLikeDto, int>
{
    private readonly IGenericRepository<UserPostLike, int> _repository;
    private readonly IMapper _mapper;

    public UserPostLikeService(IGenericRepository<UserPostLike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<UserPostLikeDto> CreateAsync(UserPostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostLikeDto), $"The {nameof(UserPostLikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<UserPostLikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostLikeDto>>(allData);

        return result;
    }

    public async Task<UserPostLikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostLikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostLikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<UserPostLikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(UserPostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostLikeDto), $"The {nameof(UserPostLikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<UserPostLikeDto> CreateInternalAsync(UserPostLikeDto item)
    {
        var map = _mapper.Map<UserPostLike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostLikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UserPostLikeDto item)
    {
        var map = _mapper.Map<UserPostLike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}