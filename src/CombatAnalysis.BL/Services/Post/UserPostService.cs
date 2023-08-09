using AutoMapper;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Post;

internal class UserPostService : IService<UserPostDto, int>
{
    private readonly IGenericRepository<UserPost, int> _repository;
    private readonly IMapper _mapper;
    private readonly ISqlContextService _sqlContextService;
    private readonly IService<PostDto, int> _postService;

    public UserPostService(IGenericRepository<UserPost, int> repository, IMapper mapper,
        ISqlContextService sqlContextService, IService<PostDto, int> postService)
    {
        _repository = repository;
        _mapper = mapper;
        _sqlContextService = sqlContextService;
        _postService = postService;
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
        using var transaction = await _sqlContextService.BeginTransactionAsync(true);
        try
        {
            await DeletePostsAsync(id);
            transaction.CreateSavepoint("BeforeDeleteUserPost");

            var rowsAffected = await _repository.DeleteAsync(id);

            await transaction.CommitAsync();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteUserPost");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeleteUserPost");

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
        var map = _mapper.Map<UserPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UserPostDto item)
    {
        var map = _mapper.Map<UserPost>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeletePostsAsync(int userPostId)
    {
        var userPost = await _repository.GetByIdAsync(userPostId);
        await _postService.DeleteAsync(userPost.PostId);
    }
}
