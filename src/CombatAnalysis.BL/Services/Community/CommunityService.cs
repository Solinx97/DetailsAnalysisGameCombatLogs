using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityService : IService<CommunityDto, int>
{
    private readonly IGenericRepository<DAL.Entities.Community.Community, int> _communityRepository;
    private readonly IGenericRepository<CommunityUser, int> _communityUserRepository;
    private readonly IGenericRepository<CommunityPost, int> _communityPostRepository;
    private readonly IGenericRepository<DAL.Entities.Post.Post, int> _postRepository;
    private readonly ISqlContextService _sqlContextService;
    private readonly IMapper _mapper;

    public CommunityService(IGenericRepository<DAL.Entities.Community.Community, int> communityRepository, IGenericRepository<CommunityUser, int> communityUserRepository,
        IGenericRepository<CommunityPost, int> communityPostRepository, IGenericRepository<DAL.Entities.Post.Post, int> postRepository,
        ISqlContextService sqlContextService, IMapper mapper)
    {
        _communityRepository = communityRepository;
        _communityUserRepository = communityUserRepository;
        _communityPostRepository = communityPostRepository;
        _postRepository = postRepository;
        _sqlContextService = sqlContextService;
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
        using var transaction = await _sqlContextService.BeginTransactionAsync();
        try
        {
            await DeleteCommunityUsersAsync(id);
            await DeleteCommunityPostsAsync(id);

            var rowsAffected = await _communityRepository.DeleteAsync(id);

            await transaction.CommitAsync();

            return rowsAffected;
        }
        catch (ArgumentException)
        {
            await transaction.RollbackAsync();

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return 0;
        }
    }

    public async Task<IEnumerable<CommunityDto>> GetAllAsync()
    {
        var allData = await _communityRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDto>>(allData);

        return result;
    }

    public async Task<CommunityDto> GetByIdAsync(int id)
    {
        var result = await _communityRepository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _communityRepository.GetByParam(paramName, value));
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
        var createdItem = await _communityRepository.CreateAsync(map);
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
        var rowsAffected = await _communityRepository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeleteCommunityUsersAsync(int communityId)
    {
        var communityUsers = await GetCommunityUsersAsync(communityId);
        foreach (var item in communityUsers)
        {
            var rowsAffected = await _communityUserRepository.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Community user didn't removed");
            }
        }
    }

    private async Task<IEnumerable<CommunityUserDto>> GetCommunityUsersAsync(int communityId)
    {
        var result = await Task.Run(() => _communityUserRepository.GetByParam(nameof(CommunityUser.CommunityId), communityId));
        var resultMap = _mapper.Map<IEnumerable<CommunityUserDto>>(result);

        return resultMap;
    }

    private async Task DeleteCommunityPostsAsync(int communityId)
    {
        var communityPosts = await GetCommunityPostsAsync(communityId);
        foreach (var item in communityPosts)
        {
            var rowsAffected = await _postRepository.DeleteAsync(item.PostId);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Post didn't removed");
            }

            rowsAffected = await _communityPostRepository.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Community post didn't removed");
            }
        }
    }

    private async Task<IEnumerable<CommunityPostDto>> GetCommunityPostsAsync(int communityId)
    {
        var result = await Task.Run(() => _communityPostRepository.GetByParam(nameof(CommunityPost.CommunityId), communityId));
        var resultMap = _mapper.Map<IEnumerable<CommunityPostDto>>(result);

        return resultMap;
    }
}
