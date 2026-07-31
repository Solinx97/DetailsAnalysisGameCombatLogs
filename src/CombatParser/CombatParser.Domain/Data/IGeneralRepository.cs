namespace CombatParser.Domain.Data;

public interface IGeneralRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetCreatorNamesAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetUniqueSpellsAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<TModel>> GetAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken);
}