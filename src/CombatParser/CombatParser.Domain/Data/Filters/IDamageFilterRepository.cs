using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Interfaces.Filters;

public interface IDamageFilterRepository
{
    Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId, CancellationToken cancellationToken);
}
