using AutoMapper;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Post;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Post;

internal class CommunityPostService : IService<CommunityPostDto, int>
{
    private readonly IGenericRepository<CommunityPost, int> _repository;
    private readonly IService<PostDto, int> _postService;
    private readonly IMapper _mapper;

    public CommunityPostService(IGenericRepository<CommunityPost, int> repository, IService<PostDto, int> postService, 
        IMapper mapper)
    {
        _repository = repository;
        _postService = postService;
        _mapper = mapper;
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
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
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
        var map = _mapper.Map<CommunityPost>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityPostDto item)
    {
        var map = _mapper.Map<CommunityPost>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
