using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostService : IUserPostService
{
    private readonly IUserPostRepository _repository;
    private readonly IService<UserPostLikeDto, int> _postLikeService;
    private readonly IService<UserPostDislikeDto, int> _postDislikeService;
    private readonly IService<UserPostCommentDto, int> _postCommentService;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public UserPostService(IUserPostRepository repository, IMapper mapper,
        IService<UserPostLikeDto, int> postLikeService, IService<UserPostDislikeDto, int> postDislikeService,
        IService<UserPostCommentDto, int> postCommentService, ISqlContextService sqlContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _postLikeService = postLikeService;
        _postDislikeService = postDislikeService;
        _postCommentService = postCommentService;
        _sqlContextService = sqlContextService;
    }

    public Task<UserPostDto> CreateAsync(UserPostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostDto), $"The {nameof(UserPostDto)} can't be null");
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

            await transaction.CommitAsync();

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

    public async Task<IEnumerable<UserPostDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostDto>>(allData);

        return result;
    }

    public async Task<UserPostDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostDto>> GetByAppUserIdAsync(string appUserId, int pageSize = 100)
    {
        var result = await _repository.GetByAppUserIdAsyn(appUserId, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<UserPostDto>> GetMoreByAppUserIdAsync(string appUserId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByAppUserIdAsyn(appUserId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(result);

        return map;
    }

    public async Task<int> CountByAppUserIdAsync(string appUserId)
    {
        var count = await _repository.CountByAppUserIdAsync(appUserId);

        return count;
    }

    public Task<int> UpdateAsync(UserPostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostDto), $"The {nameof(UserPostDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<UserPostDto> CreateInternalAsync(UserPostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostDto),
                $"The property {nameof(UserPostDto.Content)} of the {nameof(UserPostDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Owner))
        {
            throw new ArgumentNullException(nameof(UserPostDto),
                $"The property {nameof(UserPostDto.Owner)} of the {nameof(UserPostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UserPostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostDto),
                $"The property {nameof(UserPostDto.Content)} of the {nameof(UserPostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPost>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeletePostLikesAsync(int postId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(nameof(UserPostLikeDto.UserPostId), postId);
        foreach (var item in postLikes)
        {
            var rowsAffected = await _postLikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(UserPostLike)} didn't removed");
            }
        }
    }

    private async Task DeletePostDislikesAsync(int postId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(nameof(UserPostDislikeDto.UserPostId), postId);
        foreach (var item in postDislikes)
        {
            var rowsAffected = await _postDislikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(UserPostDislike)} didn't removed");
            }
        }
    }

    private async Task DeletePostComentsAsync(int postId)
    {
        var postComments = await _postCommentService.GetByParamAsync(nameof(UserPostCommentDto.UserPostId), postId);
        foreach (var item in postComments)
        {
            var rowsAffected = await _postCommentService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(UserPostComment)} didn't removed");
            }
        }
    }
}
