using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class PersonalChatMessageService : IChatMessageService<PersonalChatMessageDto, int>
{
    private readonly IChatMessageRepository<PersonalChatMessage, int> _repository;
    private readonly IMapper _mapper;

    public PersonalChatMessageService(IChatMessageRepository<PersonalChatMessage, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PersonalChatMessageDto> CreateAsync(PersonalChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageDto), $"The {nameof(PersonalChatMessageDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(allData);

        return result;
    }

    public async Task<PersonalChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PersonalChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatMessageDto>> GetByChatIdAsyn(int chatId, int pageSize = 100)
    {
        var result = await _repository.GetByChatIdAsyn(chatId, pageSize);
        var map = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

        return map;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _repository.CountByChatIdAsync(chatId);

        return count;
    }

    public Task<int> UpdateAsync(PersonalChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageDto), $"The {nameof(PersonalChatMessageDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<PersonalChatMessageDto> CreateInternalAsync(PersonalChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageDto),
                $"The property {nameof(PersonalChatMessageDto.Message)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PersonalChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PersonalChatMessageDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PersonalChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageDto),
                $"The property {nameof(PersonalChatMessageDto.Message)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PersonalChatMessage>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
