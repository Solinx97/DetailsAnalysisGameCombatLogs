using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Chat;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Chat;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Chat;

internal class PersonalChatService : IService<PersonalChatDto, int>
{
    private readonly IGenericRepository<PersonalChat, int> _repository;
    private readonly IService<PersonalChatMessageDto, int> _personalChatMessageService;
    private readonly IMapper _mapper;
    private readonly ISqlContextService _sqlContextService;

    public PersonalChatService(IGenericRepository<PersonalChat, int> repository, IMapper mapper, 
        ISqlContextService sqlContextService, IService<PersonalChatMessageDto, int> personalChatMessageService)
    {
        _repository = repository;
        _mapper = mapper;
        _sqlContextService = sqlContextService;
        _personalChatMessageService = personalChatMessageService;
    }

    public Task<PersonalChatDto> CreateAsync(PersonalChatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), $"The {nameof(PersonalChatDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var transaction = await _sqlContextService.BeginTransactionAsync(false);
        try
        {
            await DeletePersonalChatMessagesAsync(id);
            transaction.CreateSavepoint("BeforeDeletePersonalChat");

            var rowsAffected = await _repository.DeleteAsync(id);

            await transaction.CommitAsync();

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePersonalChat");

            return 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackToSavepointAsync("BeforeDeletePersonalChat");

            return 0;
        }
    }

    public async Task<IEnumerable<PersonalChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PersonalChatDto>>(allData);

        return result;
    }

    public async Task<PersonalChatDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PersonalChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PersonalChatDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PersonalChatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), $"The {nameof(PersonalChatDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<PersonalChatDto> CreateInternalAsync(PersonalChatDto item)
    {
        if (string.IsNullOrEmpty(item.LastMessage))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.LastMessage)} of the {nameof(PersonalChatDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PersonalChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PersonalChatDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PersonalChatDto item)
    {
        if (string.IsNullOrEmpty(item.LastMessage))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.LastMessage)} of the {nameof(PersonalChatDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PersonalChat>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }

    private async Task DeletePersonalChatMessagesAsync(int chatId)
    {
        var perosnalChatMessages = await _personalChatMessageService.GetByParamAsync(nameof(PersonalChatMessageDto.PersonalChatId), chatId);
        foreach (var item in perosnalChatMessages)
        {
            var rowsAffected = await _personalChatMessageService.DeleteAsync(item.Id);
            if (rowsAffected == 0)
            {
                throw new ArgumentException("Personal chat message didn't removed");
            }
        }
    }
}
