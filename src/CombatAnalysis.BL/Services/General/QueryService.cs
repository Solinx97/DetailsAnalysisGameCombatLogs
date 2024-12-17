using AutoMapper;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services.General;

internal class QueryService<TModel, TModelMap> : IQueryService<TModel>
    where TModel : class
    where TModelMap : class, IEntity
{
    private readonly IGenericRepository<TModelMap> _repository;
    private readonly IMapper _mapper;

    public QueryService(IGenericRepository<TModelMap> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<TModel>>(allData);

        return result;
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<TModel>(result);

        return resultMap;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value)
    {
        var result = await _repository.GetByParamAsync(paramName, value);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
