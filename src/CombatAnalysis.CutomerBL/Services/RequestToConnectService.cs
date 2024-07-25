using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class RequestToConnectService : IService<RequestToConnectDto, int>
{
    private readonly IGenericRepository<RequestToConnect, int> _repository;
    private readonly IMapper _mapper;

    public RequestToConnectService(IGenericRepository<RequestToConnect, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<RequestToConnectDto> CreateAsync(RequestToConnectDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(RequestToConnectDto), $"The {nameof(RequestToConnectDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<RequestToConnectDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<RequestToConnectDto>>(allData);

        return result;
    }

    public async Task<RequestToConnectDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<RequestToConnectDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<RequestToConnectDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<RequestToConnectDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(RequestToConnectDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(RequestToConnectDto), $"The {nameof(RequestToConnectDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<RequestToConnectDto> CreateInternalAsync(RequestToConnectDto item)
    {
        var map = _mapper.Map<RequestToConnect>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<RequestToConnectDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(RequestToConnectDto item)
    {
        var map = _mapper.Map<RequestToConnect>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
