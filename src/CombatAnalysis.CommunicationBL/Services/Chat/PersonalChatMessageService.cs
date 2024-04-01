using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Chat;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Chat;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Chat;

internal class PersonalChatMessageService : IService<PersonalChatMessageDto, int>
{
    private readonly IGenericRepository<PersonalChatMessage, int> _repository;
    private readonly IMapper _mapper;

    public PersonalChatMessageService(IGenericRepository<PersonalChatMessage, int> repository, IMapper mapper)
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
