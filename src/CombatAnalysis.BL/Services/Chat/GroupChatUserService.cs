using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class GroupChatUserService : IService<GroupChatUserDto, int>
{
    private readonly IGenericRepository<GroupChatUser, int> _repository;
    private readonly IMapper _mapper;

    public GroupChatUserService(IGenericRepository<GroupChatUser, int> repository, IMapper mapper)
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

    public async Task<int> DeleteAsync(int id)
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

    public async Task<GroupChatUserDto> GetByIdAsync(int id)
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
        var map = _mapper.Map<GroupChatUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatUserDto item)
    {
        var map = _mapper.Map<GroupChatUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
