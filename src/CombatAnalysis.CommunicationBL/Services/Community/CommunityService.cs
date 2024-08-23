using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityService : IService<CommunityDto, int>
{
    private readonly IGenericRepository<CommunicationDAL.Entities.Community.Community, int> _repository;
    private readonly IService<CommunityUserDto, string> _communityUserService;
    private readonly IService<InviteToCommunityDto, int> _inviteToCommunityService;
    private readonly ICommunityPostService _postService;
    private readonly IService<CommunityPostCommentDto, int> _postCommentService;
    private readonly IService<CommunityPostLikeDto, int> _postLikeService;
    private readonly IService<CommunityPostDislikeDto, int> _postDislikeService;
    private readonly IService<CommunityDiscussionDto, int> _communityDiscussionService;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public CommunityService(IGenericRepository<CommunicationDAL.Entities.Community.Community, int> communityRepository, ISqlContextService sqlContextService,
        IService<InviteToCommunityDto, int> inviteToCommunityService, IService<CommunityUserDto, string> communityUserService,
        ICommunityPostService postService, IService<CommunityPostCommentDto, int> postCommentService,
        IService<CommunityPostLikeDto, int> postLikeService, IService<CommunityPostDislikeDto, int> postDislikeService,
        IService<CommunityDiscussionDto, int> communityDiscussionService, IMapper mapper)
    {
        _repository = communityRepository;
        _sqlContextService = sqlContextService;
        _inviteToCommunityService = inviteToCommunityService;
        _communityUserService = communityUserService;
        _postService = postService;
        _postCommentService = postCommentService;
        _postLikeService = postLikeService;
        _postDislikeService = postDislikeService;
        _communityDiscussionService = communityDiscussionService;
        _mapper = mapper;
    }

    public Task<CommunityDto> CreateAsync(CommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDto), $"The {nameof(CommunityDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync(true);
        try
        {
            await DeleteCommunityPostsAsync(id);
            await DeleteCommunityDiscussionsAsync(id);
            await DeleteInvitesToCommunityAsync(id);
            await DeleteCommunityUsersAsync(id);

            transaction.CreateSavepoint("BeforeDeleteCommunity");

            var rowsAffected = await _repository.DeleteAsync(id);

            await transaction.CommitAsync();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteCommunity");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteCommunity");

            return 0;
        }
    }

    public async Task<IEnumerable<CommunityDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDto>>(allData);

        return result;
    }

    public async Task<CommunityDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDto), $"The {nameof(CommunityDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityDto> CreateInternalAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunicationDAL.Entities.Community.Community>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunicationDAL.Entities.Community.Community>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteCommunityPostsAsync(int communityId)
    {
        var posts = await _postService.GetByParamAsync(nameof(CommunityPostDto.CommunityId), communityId);
        foreach (var item in posts)
        {
            await DeleteCommunityPostCommentsAsync(item.Id);
            await DeleteCommunityPostLikesAsync(item.Id);
            await DeleteCommunityPostDislikesAsync(item.Id);

            var rowsAffected = await _postService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostDto)} didn't removed");
            }
        }
    }

    private async Task DeleteCommunityPostCommentsAsync(int communityPostId)
    {
        var postComments = await _postCommentService.GetByParamAsync(nameof(CommunityPostCommentDto.CommunityPostId), communityPostId);
        foreach (var item in postComments)
        {
            var rowsAffected = await _postService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostCommentDto)} didn't removed");
            }
        }
    }

    private async Task DeleteCommunityPostLikesAsync(int communityPostId)
    {
        var postLikes = await _postLikeService.GetByParamAsync(nameof(CommunityPostLikeDto.CommunityPostId), communityPostId);
        foreach (var item in postLikes)
        {
            var rowsAffected = await _postService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostLikeDto)} didn't removed");
            }
        }
    }

    private async Task DeleteCommunityPostDislikesAsync(int communityPostId)
    {
        var postDislikes = await _postDislikeService.GetByParamAsync(nameof(CommunityPostDislikeDto.CommunityPostId), communityPostId);
        foreach (var item in postDislikes)
        {
            var rowsAffected = await _postService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityPostDislikeDto)} didn't removed");
            }
        }
    }

    private async Task DeleteCommunityDiscussionsAsync(int communityId)
    {
        var communityDiscussions = await _communityDiscussionService.GetByParamAsync(nameof(CommunityDiscussionDto.CommunityId), communityId);
        foreach (var item in communityDiscussions)
        {
            var rowsAffected = await _communityDiscussionService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityDiscussionDto)} didn't removed");
            }
        }
    }

    private async Task DeleteInvitesToCommunityAsync(int communityId)
    {
        var invitesToCommunity = await _inviteToCommunityService.GetByParamAsync(nameof(InviteToCommunityDto.CommunityId), communityId);
        foreach (var item in invitesToCommunity)
        {
            var rowsAffected = await _inviteToCommunityService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(InviteToCommunityDto)} didn't removed");
            }
        }
    }

    private async Task DeleteCommunityUsersAsync(int communityId)
    {
        var communityUsers = await _communityUserService.GetByParamAsync(nameof(CommunityUserDto.CommunityId), communityId);
        foreach (var item in communityUsers)
        {
            var rowsAffected = await _communityUserService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException($"{nameof(CommunityUserDto)} didn't removed");
            }
        }
    }
}
