using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IUnitPositionRepository
{
    Task<IDictionary<string, IEnumerable<UnitPosition>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
