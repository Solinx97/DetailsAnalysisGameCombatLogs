using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatRepository : IGenericRepository<Combat, int>
{
    Task AddBulkAsync(Combat item, CancellationToken cancelationToken);

    Task<IEnumerable<Combat>> GetByCombatLogId(int combatLogId, CancellationToken cancelationToken);
}
