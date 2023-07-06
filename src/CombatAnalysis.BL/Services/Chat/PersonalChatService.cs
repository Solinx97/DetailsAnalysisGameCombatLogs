using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class PersonalChatService : IService<PersonalChatDto, int>
{
    private readonly IGenericRepository<PersonalChat, int> _repository;
    private readonly IMapper _mapper;

    public PersonalChatService(IGenericRepository<PersonalChat, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
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
        if (string.IsNullOrEmpty(item.InitiatorUsername))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.InitiatorUsername)} of the {nameof(PersonalChatDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.CompanionUsername))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.CompanionUsername)} of the {nameof(PersonalChatDto)} object can't be null or empty");
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
        if (string.IsNullOrEmpty(item.InitiatorUsername))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.InitiatorUsername)} of the {nameof(PersonalChatDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.CompanionUsername))
        {
            throw new ArgumentNullException(nameof(PersonalChatDto), 
                $"The property {nameof(PersonalChatDto.CompanionUsername)} of the {nameof(PersonalChatDto)} object can't be null or empty");
        }

        var map = _mapper.Map<PersonalChat>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
