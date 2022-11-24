using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class GroupChatMessageService : IService<GroupChatMessageDto, int>
{
    private readonly IGenericRepository<GroupChatMessage, int> _repository;
    private readonly IMapper _mapper;

    public GroupChatMessageService(IGenericRepository<GroupChatMessage, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<GroupChatMessageDto> CreateAsync(GroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), $"The {nameof(GroupChatMessageDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public Task<int> DeleteAsync(GroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), $"The {nameof(GroupChatMessageDto)} can't be null");
        }

        return DeleteInternalAsync(item);
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatMessageDto>>(allData);

        return result;
    }

    public async Task<GroupChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), $"The {nameof(GroupChatMessageDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatMessageDto> CreateInternalAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto),
                $"The property {nameof(GroupChatMessageDto.Username)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Message)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatMessageDto>(createdItem);

        return resultMap;
    }

    private async Task<int> DeleteInternalAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Username)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Message)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var rowsAffected = await _repository.DeleteAsync(map);

        return rowsAffected;
    }

    private async Task<int> UpdateInternalAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto),
                $"The property {nameof(GroupChatMessageDto.Username)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Message)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
