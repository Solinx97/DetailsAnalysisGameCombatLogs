using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityPostCommentService : IService<CommunityPostCommentDto, int>
{
    private readonly IGenericRepository<CommunityPostComment, int> _repository;
    private readonly IMapper _mapper;

    public CommunityPostCommentService(IGenericRepository<CommunityPostComment, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityPostCommentDto> CreateAsync(CommunityPostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostCommentDto), $"The {nameof(CommunityPostCommentDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityPostCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostCommentDto>>(allData);

        return result;
    }

    public async Task<CommunityPostCommentDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostCommentDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityPostCommentDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityPostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostCommentDto), $"The {nameof(CommunityPostCommentDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<CommunityPostCommentDto> CreateInternalAsync(CommunityPostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityPostCommentDto),
                $"The property {nameof(CommunityPostCommentDto.Content)} of the {nameof(CommunityPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityPostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostCommentDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityPostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityPostCommentDto),
                $"The property {nameof(CommunityPostCommentDto.Content)} of the {nameof(CommunityPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityPostComment>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
