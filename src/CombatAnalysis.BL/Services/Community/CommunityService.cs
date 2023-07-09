using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityService : IService<CommunityDto, int>
{
    private readonly IGenericRepository<DAL.Entities.Community.Community, int> _repository;
    private readonly IMapper _mapper;

    public CommunityService(IGenericRepository<DAL.Entities.Community.Community, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityDto> CreateAsync(CommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDto), $"The {nameof(CommunityDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityDto>>(allData);

        return result;
    }

    public async Task<CommunityDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityDto), $"The {nameof(CommunityDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityDto> CreateInternalAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DAL.Entities.Community.Community>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityDto item)
    {
        if (string.IsNullOrEmpty(item.Name))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Name)} of the {nameof(CommunityDto)} object can't be null or empty");
        }
        if (string.IsNullOrEmpty(item.Description))
        {
            throw new ArgumentNullException(nameof(CommunityDto),
                $"The property {nameof(CommunityDto.Description)} of the {nameof(CommunityDto)} object can't be null or empty");
        }

        var map = _mapper.Map<DAL.Entities.Community.Community>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
