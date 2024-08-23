using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Post;

internal class UserPostCommentService : IService<UserPostCommentDto, int>
{
    private readonly IGenericRepository<UserPostComment, int> _repository;
    private readonly IMapper _mapper;

    public UserPostCommentService(IGenericRepository<UserPostComment, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<UserPostCommentDto> CreateAsync(UserPostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto), $"The {nameof(UserPostCommentDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<UserPostCommentDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<UserPostCommentDto>>(allData);

        return result;
    }

    public async Task<UserPostCommentDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<UserPostCommentDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<UserPostCommentDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<UserPostCommentDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(UserPostCommentDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto), $"The {nameof(UserPostCommentDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<UserPostCommentDto> CreateInternalAsync(UserPostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto),
                $"The property {nameof(UserPostCommentDto.Content)} of the {nameof(UserPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPostComment>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<UserPostCommentDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(UserPostCommentDto item)
    {
        if (string.IsNullOrEmpty(item.Content))
        {
            throw new ArgumentNullException(nameof(UserPostCommentDto),
                $"The property {nameof(UserPostCommentDto.Content)} of the {nameof(UserPostCommentDto)} object can't be null or empty");
        }

        var map = _mapper.Map<UserPostComment>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
