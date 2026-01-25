using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatLogRepository : IGenericRepository<CombatLog, int>
{
    Task DeleteAsync(int id, CancellationToken ct = default);
}
