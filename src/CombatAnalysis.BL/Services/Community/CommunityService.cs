using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityService : IService<CommunityDto, int>
{
    private readonly IGenericRepository<DAL.Entities.Community.Community, int> _repository;
    private readonly IService<CommunityUserDto, int> _communityUserService;
    private readonly IService<InviteToCommunityDto, int> _inviteToCommunityService;
    private readonly IService<CommunityPostDto, int> _communityPostService;
    private readonly IService<PostDto, int> _postService;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public CommunityService(IGenericRepository<DAL.Entities.Community.Community, int> communityRepository, ISqlContextService sqlContextService,
        IService<InviteToCommunityDto, int> inviteToCommunityService, IService<CommunityUserDto, int> communityUserService,
        IService<CommunityPostDto, int> communityPostService, IService<PostDto, int> postService,
        IMapper mapper)
    {
        _repository = communityRepository;
        _sqlContextService = sqlContextService;
        _inviteToCommunityService = inviteToCommunityService;
        _communityUserService = communityUserService;
        _communityPostService = communityPostService;
        _postService = postService;
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
            await DeletePostsAsync(id);
            await DeleteCommunityUsersAsync(id);
            await DeleteInvitesToCommunityAsync(id);
            await DeleteCommunityPostsAsync(id);
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

        var map = _mapper.Map<DAL.Entities.Community.Community>(item);
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

        var map = _mapper.Map<DAL.Entities.Community.Community>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteCommunityUsersAsync(int communityId)
    {
        var communityUsers = await _communityUserService.GetByParamAsync(nameof(CommunityUserDto.CommunityId), communityId);
        foreach (var item in communityUsers)
        {
            var rowsAffected = await _communityUserService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Community user didn't removed");
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
                throw new ArgumentException("Invite to community didn't removed");
            }
        }
    }

    private async Task DeletePostsAsync(int communityId)
    {
        var communityPosts = await _communityPostService.GetByParamAsync(nameof(CommunityPostDto.CommunityId), communityId);
        foreach (var item in communityPosts)
        {
            var rowsAffected = await _postService.DeleteAsync(item.PostId);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Post didn't removed");
            }
        }
    }

    private async Task DeleteCommunityPostsAsync(int communityId)
    {
        var communityPosts = await _communityPostService.GetByParamAsync(nameof(CommunityPostDto.CommunityId), communityId);
        foreach (var item in communityPosts)
        {
            var rowsAffected = await _communityPostService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Community post didn't removed");
            }
        }
    }
}
