using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Chat;

internal class PersonalChatMessageCountService : IService<PersonalChatMessageCountDto, int>
{
    private readonly IGenericRepository<PersonalChatMessageCount, int> _repository;
    private readonly IMapper _mapper;

    public PersonalChatMessageCountService(IGenericRepository<PersonalChatMessageCount, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<PersonalChatMessageCountDto> CreateAsync(PersonalChatMessageCountDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageCountDto), $"The {nameof(PersonalChatMessageCountDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<PersonalChatMessageCountDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PersonalChatMessageCountDto>>(allData);

        return result;
    }

    public async Task<PersonalChatMessageCountDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<PersonalChatMessageCountDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<PersonalChatMessageCountDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<PersonalChatMessageCountDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(PersonalChatMessageCountDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(PersonalChatMessageCountDto), $"The {nameof(PersonalChatMessageCountDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<PersonalChatMessageCountDto> CreateInternalAsync(PersonalChatMessageCountDto item)
    {
        var map = _mapper.Map<PersonalChatMessageCount>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<PersonalChatMessageCountDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(PersonalChatMessageCountDto item)
    {
        var map = _mapper.Map<PersonalChatMessageCount>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
