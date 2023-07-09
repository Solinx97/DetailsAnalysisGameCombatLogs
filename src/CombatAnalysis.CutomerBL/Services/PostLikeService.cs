using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class PostLikeService : IService<PostLikeDto, int>
{
    private readonly IGenericRepository<PostLike, int> _repository;
    private readonly IMapper _mapper;

    public PostLikeService(IGenericRepository<PostLike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PostLikeDto> CreateAsync(PostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostLikeDto), $"The {nameof(PostLikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PostLikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<PostLikeDto>>(allData);

        return result;
    }

    public async Task<PostLikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PostLikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PostLikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PostLikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostLikeDto), $"The {nameof(PostLikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<PostLikeDto> CreateInternalAsync(PostLikeDto item)
    {
        var map = _mapper.Map<PostLike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PostLikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PostLikeDto item)
    {
        var map = _mapper.Map<PostLike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}