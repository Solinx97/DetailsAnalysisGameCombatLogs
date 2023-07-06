using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class PostService : IService<PostDto, int>
{
    private readonly IGenericRepository<Post, int> _repository;
    private readonly IMapper _mapper;

    public PostService(IGenericRepository<Post, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PostDto> CreateAsync(PostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostDto), $"The {nameof(PostDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PostDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<PostDto>>(allData);

        return result;
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PostDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PostDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PostDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostDto), $"The {nameof(PostDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<PostDto> CreateInternalAsync(PostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(PostDto),
                $"The property {nameof(PostDto.Content)} of the {nameof(PostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Post>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PostDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(PostDto),
                $"The property {nameof(PostDto.Content)} of the {nameof(PostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Post>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
