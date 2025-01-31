using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class VoiceChatService : IService<VoiceChatDto, string>
{
    private readonly IGenericRepository<VoiceChat, string> _repository;
    private readonly IMapper _mapper;

    public VoiceChatService(IGenericRepository<VoiceChat, string> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<VoiceChatDto> CreateAsync(VoiceChatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(VoiceChatDto), $"The {nameof(VoiceChatDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<VoiceChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<VoiceChatDto>>(allData);

        return result;
    }

    public async Task<VoiceChatDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<VoiceChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<VoiceChatDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<VoiceChatDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(VoiceChatDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(VoiceChatDto), $"The {nameof(VoiceChatDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<VoiceChatDto> CreateInternalAsync(VoiceChatDto item)
    {
        var map = _mapper.Map<VoiceChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<VoiceChatDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(VoiceChatDto item)
    {
        var map = _mapper.Map<VoiceChat>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
