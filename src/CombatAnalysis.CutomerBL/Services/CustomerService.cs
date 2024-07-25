using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class CustomerService : IService<CustomerDto, string>
{
    private readonly IGenericRepository<Customer, string> _repository;
    private readonly IMapper _mapper;

    public CustomerService(IGenericRepository<Customer, string> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CustomerDto> CreateAsync(CustomerDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CustomerDto), $"The {nameof(CustomerDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CustomerDto>>(allData);

        return result;
    }

    public async Task<CustomerDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CustomerDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CustomerDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<CustomerDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CustomerDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CustomerDto), $"The {nameof(CustomerDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }


    private async Task<CustomerDto> CreateInternalAsync(CustomerDto item)
    {
        if (string.IsNullOrEmpty(item.City))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.City)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Country))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.Country)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Customer>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CustomerDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CustomerDto item)
    {
        if (string.IsNullOrEmpty(item.City))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.City)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Country))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.Country)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Customer>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
