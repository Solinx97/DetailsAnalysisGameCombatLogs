namespace CombatParser.Domain.Data.Filters;

public interface IGeneralFilterRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountDamageDoneByTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> GetValueToTargetByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetUniqueSpellsByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountDamageDoneBySpellAsync(int combatPlayerId, string spell, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken);
}