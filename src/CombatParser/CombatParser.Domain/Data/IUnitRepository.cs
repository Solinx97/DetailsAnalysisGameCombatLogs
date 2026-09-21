using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IUnitRepository
{
    Task<IEnumerable<Unit>> GetAsync(int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<UniqueUnitName>> GetUniqueNamesAsync(int combatLogId, string bossName, CancellationToken cancellationToken);

    Task<IEnumerable<UnitPosition>> GetPositionsAsync(string combatUnitId, CancellationToken cancellationToken);

    Task<IEnumerable<UnitCast>> GetCastsAsync(string combatUnitId, CancellationToken cancellationToken);

    Task<IDictionary<string, List<UnitHealth>>> GetUnitsHealthAsync(int combatId, CancellationToken cancellationToken);

    Task<List<UnitHealth>> GetUnitsHealthByIntervalAsync(string unitId, string from, string to, CancellationToken cancellationToken);
}
