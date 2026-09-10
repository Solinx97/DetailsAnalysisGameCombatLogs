namespace CombatParser.Domain.Data;

public interface IGeneralRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetCreatorNamesAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetUniqueSpellsAsync(int combatPlayerId, CancellationToken cancellationToken, int[]? creatorTypes = null, int[]? targetTypes = null);

    Task<IEnumerable<TModel>> GetAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken, int? targetType = null, int? creatorType = null);

    Task<int> CountAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken);
}