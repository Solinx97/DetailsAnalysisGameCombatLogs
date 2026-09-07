using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatLogRepository : IGenericRepository<CombatLog, int>
{
    Task<IEnumerable<CombatLog>> GetByLogTypeAsync(int logType, int gameVersion, string? appUserId, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
