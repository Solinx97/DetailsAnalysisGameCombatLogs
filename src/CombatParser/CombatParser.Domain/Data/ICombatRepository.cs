using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatRepository
{
    Task AddBulkAsync(Combat item, CancellationToken cancellationToken);

    Task<IEnumerable<Combat>> GetByCombatLogIdAsync(int combatLogId, CancellationToken cancellationToken);

    Task<Combat?> GetByIdAsync(int combatId, CancellationToken cancellationToken);
}
