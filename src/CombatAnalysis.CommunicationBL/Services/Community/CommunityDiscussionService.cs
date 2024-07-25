using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityDiscussionService : IService<CommunityDiscussionDto, int>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository;
    private readonly IMapper _mapper;

    public CommunityDiscussionService(IGenericRepository<CommunityDiscussion, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityDiscussionDto> CreateAsync(CommunityDiscussionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto), $"The {nameof(CommunityDiscussionDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityDiscussionDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(allData);

        return result;
    }

    public async Task<CommunityDiscussionDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDiscussionDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDiscussionDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityDiscussionDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto), $"The {nameof(CommunityDiscussionDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityDiscussionDto> CreateInternalAsync(CommunityDiscussionDto item)
    {
        if (string.IsNullOrEmpty(item.Title))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto),
                $"The property {nameof(CommunityDiscussionDto.Title)} of the {nameof(CommunityDiscussionDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto),
                $"The property {nameof(CommunityDiscussionDto.Content)} of the {nameof(CommunityDiscussionDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityDiscussion>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDiscussionDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityDiscussionDto item)
    {
        if (string.IsNullOrEmpty(item.Title))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto),
                $"The property {nameof(CommunityDiscussionDto.Title)} of the {nameof(CommunityDiscussionDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(CommunityDiscussionDto),
                $"The property {nameof(CommunityDiscussionDto.Content)} of the {nameof(CommunityDiscussionDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityDiscussion>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
