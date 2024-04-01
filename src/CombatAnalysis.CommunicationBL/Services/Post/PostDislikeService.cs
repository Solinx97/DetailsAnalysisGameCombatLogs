using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class PostDislikeService : IService<PostDislikeDto, int>
{
    private readonly IGenericRepository<PostDislike, int> _repository;
    private readonly IMapper _mapper;

    public PostDislikeService(IGenericRepository<PostDislike, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PostDislikeDto> CreateAsync(PostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostDislikeDto), $"The {nameof(PostDislikeDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PostDislikeDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<PostDislikeDto>>(allData);

        return result;
    }

    public async Task<PostDislikeDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PostDislikeDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PostDislikeDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PostDislikeDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PostDislikeDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PostDislikeDto), $"The {nameof(PostDislikeDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<PostDislikeDto> CreateInternalAsync(PostDislikeDto item)
    {
        var map = _mapper.Map<PostDislike>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PostDislikeDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PostDislikeDto item)
    {
        var map = _mapper.Map<PostDislike>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
