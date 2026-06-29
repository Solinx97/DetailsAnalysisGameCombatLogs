namespace CombatParser.Domain.Data.Filters;

public interface IGeneralFilterRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountByTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> GetValueToTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetCreatorNamesAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<int> CountByCreatorAsync(int combatPlayerId, string creator, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetUniqueSpellsAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountBySpellAsync(int combatPlayerId, string spell, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetByAllAsync(int combatPlayerId, string target, string spell, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountByAllAsync(int combatPlayerId, string target, string spell, CancellationToken cancellationToken);
}