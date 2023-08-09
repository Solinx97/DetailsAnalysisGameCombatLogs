using AutoMapper;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Interfaces;

namespace CombatAnalysis.BL.Services.Community;

internal class CommunityUserService : IService<CommunityUserDto, int>
{
    private readonly IGenericRepository<CommunityUser, int> _repository;
    private readonly IMapper _mapper;

    public CommunityUserService(IGenericRepository<CommunityUser, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityUserDto> CreateAsync(CommunityUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityUserDto), $"The {nameof(CommunityUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityUserDto>>(allData);

        return result;
    }

    public async Task<CommunityUserDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityUserDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CommunityUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityUserDto), $"The {nameof(CommunityUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityUserDto> CreateInternalAsync(CommunityUserDto item)
    {
        var map = _mapper.Map<CommunityUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityUserDto item)
    {
        var map = _mapper.Map<CommunityUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
