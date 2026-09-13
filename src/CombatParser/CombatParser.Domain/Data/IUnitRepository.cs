using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IUnitRepository
{
    Task<IEnumerable<Unit>> GetAsync(int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<UnitPosition>> GetPositionsAsync(string combatUnitId, CancellationToken cancellationToken);

    Task<IEnumerable<UnitCast>> GetCastsAsync(string combatUnitId, CancellationToken cancellationToken);

    Task<IDictionary<string, List<UnitHealth>>> GetHealthesAsync(int combatId, CancellationToken cancellationToken);
}
