using AutoMapper;
using CombatAnalysis.BL.Interfaces.Filters;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;

namespace CombatAnalysis.BL.Services.Filters;

internal class GeneralFilterService<TModel, TModelMap> : IGeneralFilterService<TModel>
    where TModel : class, IGeneralFilterEntity
    where TModelMap : class, IGeneralFilterEntity
{
    private readonly IMapper _mapper;
    private readonly IGeneralFilter<TModelMap> _repository;

    public GeneralFilterService(IGeneralFilter<TModelMap> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var result = await _repository.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

        return result;
    }

    public async Task<int> CountTargetsByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var count = await _repository.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetTargetsByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var result = await _repository.GetTargetsByCombatPlayerIdAsync(combatPlayerId, target, page, pageSize);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
