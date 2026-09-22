using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface IGeneralRepository<TModel>
    where TModel : class
{
    Task<IEnumerable<string>> GetUniqueTargetsByCreatorIdAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null);

    Task<IEnumerable<string>> GetUniqueCreatorsByTargetIdAsync(string unitId, CancellationToken cancellationToken, int[]? creatorTypes = null);

    Task<IEnumerable<string>> GetUniqueTargetSpellsByCreatorIdAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null);

    Task<IEnumerable<string>> GetUniqueCreatorSpellsByTargetIdAsync(string unitId, CancellationToken cancellationToken, int[]? creatorTypes = null);

    Task<IEnumerable<TModel>> GetAsync(string unitId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null);

    Task<IEnumerable<DamageDone>> GetDamageTakenAsync(string targetId, int combatId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null);

    Task<int> CountAsync(string unitId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken);
}