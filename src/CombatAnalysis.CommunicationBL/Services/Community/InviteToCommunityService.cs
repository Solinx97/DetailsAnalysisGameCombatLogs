using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;

namespace CombatAnalysis.CommunicationBL.Services.Community;
internal class InviteToCommunityService : IService<InviteToCommunityDto, int>
{
    private readonly IGenericRepository<InviteToCommunity, int> _repository;
    private readonly IMapper _mapper;

    public InviteToCommunityService(IGenericRepository<InviteToCommunity, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<InviteToCommunityDto> CreateAsync(InviteToCommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(InviteToCommunityDto), $"The {nameof(InviteToCommunityDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<InviteToCommunityDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<InviteToCommunityDto>>(allData);

        return result;
    }

    public async Task<InviteToCommunityDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<InviteToCommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<InviteToCommunityDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<InviteToCommunityDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(InviteToCommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(InviteToCommunityDto), $"The {nameof(InviteToCommunityDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<InviteToCommunityDto> CreateInternalAsync(InviteToCommunityDto item)
    {
        var map = _mapper.Map<InviteToCommunity>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<InviteToCommunityDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(InviteToCommunityDto item)
    {
        var map = _mapper.Map<InviteToCommunity>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
