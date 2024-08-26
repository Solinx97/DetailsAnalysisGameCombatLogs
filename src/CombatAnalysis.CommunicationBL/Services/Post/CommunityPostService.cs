using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostService : ICommunityPostService
{
    private readonly ICommunityPostRepository _repository;
    private readonly IService<CommunityPostLikeDto, int> _postLikeService;
    private readonly IService<CommunityPostDislikeDto, int> _postDislikeService;
    private readonly IService<CommunityPostCommentDto, int> _postCommentService;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public CommunityPostService(ICommunityPostRepository repository, IMapper mapper,
        IService<CommunityPostLikeDto, int> postLikeService, IService<CommunityPostDislikeDto, int> postDislikeService,
        IService<CommunityPostCommentDto, int> postCommentService, ISqlContextService sqlContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _postLikeService = postLikeService;
        _postDislikeService = postDislikeService;
        _postCommentService = postCommentService;
        _sqlContextService = sqlContextService;
    }

    public Task<CommunityPostDto> CreateAsync(CommunityPostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostDto), $"The {nameof(CommunityPostDto)} can't be null");
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

    public async Task<IEnumerable<CommunityPostDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostDto>>(allData);

        return result;
    }

    public async Task<CommunityPostDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetByCommunityIdAsync(int communityId, int pageSize = 100)
    {
        var result = await _repository.GetByCommunityIdAsyn(communityId, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<IEnumerable<CommunityPostDto>> GetMoreByCommunityIdAsync(int communityId, int offset = 0, int pageSize = 100)
    {
        var result = await _repository.GetMoreByCommunityIdAsyn(communityId, offset, pageSize);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return map;
    }

    public async Task<int> CountByCommunityIdAsync(int communityId)
    {
        var count = await _repository.CountByCommunityIdAsync(communityId);

        return count;
    }

    public Task<int> UpdateAsync(CommunityPostDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostDto), $"The {nameof(CommunityPostDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityPostDto> CreateInternalAsync(CommunityPostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityPostDto),
                $"The property {nameof(CommunityPostDto.Content)} of the {nameof(CommunityPostDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Owner))
        {
            throw new ArgumentNullException(nameof(CommunityPostDto),
                $"The property {nameof(CommunityPostDto.Owner)} of the {nameof(CommunityPostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityPostDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityPostDto),
                $"The property {nameof(CommunityPostDto.Content)} of the {nameof(CommunityPostDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityPost>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeletePostLikesAsync(int postId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(nameof(CommunityPostLikeDto.CommunityPostId), postId);
        foreach (var item in postLikes)
        {
            var rowsAffected = await _postLikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostLike)} didn't removed");
            }
        }
    }

    private async Task DeletePostDislikesAsync(int postId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(nameof(CommunityPostDislikeDto.CommunityPostId), postId);
        foreach (var item in postDislikes)
        {
            var rowsAffected = await _postDislikeService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostDislike)} didn't removed");
            }
        }
    }

    private async Task DeletePostComentsAsync(int postId)
    {
        var postComments = await _postCommentService.GetByParamAsync(nameof(CommunityPostCommentDto.CommunityPostId), postId);
        foreach (var item in postComments)
        {
            var rowsAffected = await _postCommentService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostComment)} didn't removed");
            }
        }
    }
}
