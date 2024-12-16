using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Filters;

public interface IGeneralFilter<TModel>
    where TModel : class, IGeneralFilterEntity
{
    Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId);

    Task<int> CountTargetsByCombatPlayerIdAsync(int combatPlayerId, string target);

    Task<IEnumerable<TModel>> GetTargetsByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize);
}