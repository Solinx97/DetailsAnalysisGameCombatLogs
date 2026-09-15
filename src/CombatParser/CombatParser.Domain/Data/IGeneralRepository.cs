namespace CombatParser.Domain.Data;

public interface IGeneralRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null);

    Task<IEnumerable<string>> GetCreatorNamesAsync(string unitId, CancellationToken cancellationToken, int[]? creatorTypes = null);

    Task<IEnumerable<string>> GetUniqueSpellsAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null);

    Task<IEnumerable<TModel>> GetAsync(string unitId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null);

    Task<int> CountAsync(string unitId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken);
}