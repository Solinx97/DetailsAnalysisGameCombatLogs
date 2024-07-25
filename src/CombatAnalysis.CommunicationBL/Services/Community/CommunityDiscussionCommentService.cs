using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityDiscussionCommentService : IService<CommunityDiscussionCommentDto, int>
{
    private readonly IGenericRepository<CommunityDiscussionComment, int> _repository;
    private readonly IMapper _mapper;

    public CommunityDiscussionCommentService(IGenericRepository<CommunityDiscussionComment, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityDiscussionCommentDto> CreateAsync(CommunityDiscussionCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionCommentDto), $"The {nameof(CommunityDiscussionCommentDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityDiscussionCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(allData);

        return result;
    }

    public async Task<CommunityDiscussionCommentDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDiscussionCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDiscussionCommentDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityDiscussionCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionCommentDto), $"The {nameof(CommunityDiscussionCommentDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityDiscussionCommentDto> CreateInternalAsync(CommunityDiscussionCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionCommentDto),
                $"The property {nameof(CommunityDiscussionCommentDto.Content)} of the {nameof(CommunityDiscussionCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityDiscussionComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDiscussionCommentDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityDiscussionCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionCommentDto),
                $"The property {nameof(CommunityDiscussionCommentDto.Content)} of the {nameof(CommunityDiscussionCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityDiscussionComment>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
