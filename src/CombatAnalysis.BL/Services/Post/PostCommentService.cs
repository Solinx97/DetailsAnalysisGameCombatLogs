using AutoMapper;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Post;

internal class PostCommentService : IService<PostCommentDto, int>
{
    private readonly IGenericRepository<PostComment, int> _repository;
    private readonly IMapper _mapper;

    public PostCommentService(IGenericRepository<PostComment, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PostCommentDto> CreateAsync(PostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostCommentDto), $"The {nameof(PostCommentDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PostCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<PostCommentDto>>(allData);

        return result;
    }

    public async Task<PostCommentDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PostCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PostCommentDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PostCommentDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostCommentDto), $"The {nameof(PostCommentDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<PostCommentDto> CreateInternalAsync(PostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(PostCommentDto),
                $"The property {nameof(PostCommentDto.Content)} of the {nameof(PostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PostCommentDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(PostCommentDto),
                $"The property {nameof(PostCommentDto.Content)} of the {nameof(PostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PostComment>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
