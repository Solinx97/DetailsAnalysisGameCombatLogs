using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Filters;

public interface IGeneralFilter<TModel>
    where TModel : class, IGeneralFilterEntity
{
    Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId);

    Task<int> CountTargetByCombatPlayerIdAsync(int combatPlayerId, string target);

    Task<IEnumerable<TModel>> GetTargetByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize);

    Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId);

    Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator);

    Task<IEnumerable<TModel>> GetCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, int page, int pageSize);

    Task<IEnumerable<string>> GetSpellNamesByCombatPlayerIdAsync(int combatPlayerId);

    Task<int> CountSpellByCombatPlayerIdAsync(int combatPlayerId, string spell);

    Task<IEnumerable<TModel>> GetSpellByCombatPlayerIdAsync(int combatPlayerId, string spell, int page, int pageSize);
}