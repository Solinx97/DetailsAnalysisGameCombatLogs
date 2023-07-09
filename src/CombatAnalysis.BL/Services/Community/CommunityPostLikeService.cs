using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityPostLikeService : IService<CommunityPostLikeDto, int>
{
    private readonly IGenericRepository<CommunityPostLike, int> _repository;
    private readonly IMapper _mapper;

    public CommunityPostLikeService(IGenericRepository<CommunityPostLike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityPostLikeDto> CreateAsync(CommunityPostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostLikeDto), $"The {nameof(CommunityPostLikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityPostLikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostLikeDto>>(allData);

        return result;
    }

    public async Task<CommunityPostLikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostLikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostLikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityPostLikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityPostLikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostLikeDto), $"The {nameof(CommunityPostLikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<CommunityPostLikeDto> CreateInternalAsync(CommunityPostLikeDto item)
    {
        var map = _mapper.Map<CommunityPostLike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostLikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityPostLikeDto item)
    {
        var map = _mapper.Map<CommunityPostLike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
