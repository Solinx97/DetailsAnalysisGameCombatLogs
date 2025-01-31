using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class CommunityPostDislikeService : IService<CommunityPostDislikeDto, int>
{
    private readonly IGenericRepository<CommunityPostDislike, int> _repository;
    private readonly IMapper _mapper;

    public CommunityPostDislikeService(IGenericRepository<CommunityPostDislike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityPostDislikeDto> CreateAsync(CommunityPostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostDislikeDto), $"The {nameof(CommunityPostDislikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityPostDislikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CommunityPostDislikeDto>>(allData);

        return result;
    }

    public async Task<CommunityPostDislikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityPostDislikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityPostDislikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityPostDislikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityPostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityPostDislikeDto), $"The {nameof(CommunityPostDislikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<CommunityPostDislikeDto> CreateInternalAsync(CommunityPostDislikeDto item)
    {
        var map = _mapper.Map<CommunityPostDislike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityPostDislikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityPostDislikeDto item)
    {
        var map = _mapper.Map<CommunityPostDislike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
