using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class GroupChatUserService : IService<GroupChatUserDto, string>
{
    private readonly IGenericRepository<GroupChatUser, string> _repository;
    private readonly IMapper _mapper;

    public GroupChatUserService(IGenericRepository<GroupChatUser, string> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<GroupChatUserDto> CreateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatUserDto>>(allData);

        return result;
    }

    public async Task<GroupChatUserDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatUserDto> CreateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatUserDto),
                $"The property {nameof(GroupChatUserDto.Username)} of the {nameof(GroupChatUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
