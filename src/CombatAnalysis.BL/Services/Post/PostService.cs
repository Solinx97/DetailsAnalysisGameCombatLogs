using AutoMapper;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Post;

internal class PostService : IService<PostDto, int>
{
    private readonly IGenericRepository<DAL.Entities.Post.Post, int> _repository;
    private readonly IService<PostLikeDto, int> _postLikeService;
    private readonly IService<PostDislikeDto, int> _postDislikeService;
    private readonly IService<PostCommentDto, int> _postCommentService;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public PostService(IGenericRepository<DAL.Entities.Post.Post, int> repository, IMapper mapper, 
        IService<PostLikeDto, int> postLikeService, IService<PostDislikeDto, int> postDislikeService,
        IService<PostCommentDto, int> postCommentService, ISqlContextService sqlContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _postLikeService = postLikeService;
        _postDislikeService = postDislikeService;
        _postCommentService = postCommentService;
        _sqlContextService = sqlContextService;
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
        var transaction = await _sqlContextService.UseTransactionAsync();
        try
        {
            await DeletePostLikesAsync(id);
            await DeletePostDislikesAsync(id);
            await DeletePostComentsAsync(id);
            transaction.CreateSavepoint("BeforeDeletePost");

            var rowsAffected = await _repository.DeleteAsync(id);
            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePost");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePost");

            return 0;
        }
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

        if (string.IsNullOrEmpty(item.Owner))
        {
            throw new ArgumentNullException(nameof(PostDto),
                $"The property {nameof(PostDto.Owner)} of the {nameof(PostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DAL.Entities.Post.Post>(item);
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

        var map = _mapper.Map<DAL.Entities.Post.Post>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeletePostLikesAsync(int postId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(nameof(PostLikeDto.PostId), postId);
        foreach (var item in postLikes)
        {
            var rowsAffected = await _postLikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Post like didn't removed");
            }
        }
    }

    private async Task DeletePostDislikesAsync(int postId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(nameof(PostDislikeDto.PostId), postId);
        foreach (var item in postDislikes)
        {
            var rowsAffected = await _postDislikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Post dislike didn't removed");
            }
        }
    }

    private async Task DeletePostComentsAsync(int postId)
    {
        var postComments = await _postCommentService.GetByParamAsync(nameof(PostCommentDto.PostId), postId);
        foreach (var item in postComments)
        {
            var rowsAffected = await _postCommentService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Post comment didn't removed");
            }
        }
    }
}
