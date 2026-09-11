using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatLogRepository(CombatParserContextOne context) : GenericRepository<CombatLog, int>(context), ICombatLogRepository
{
    private const int CONTEXT_TIMEOUT_MINUTES = 10;

    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatLog>> GetByLogTypeAsync(int logType, int gameVersion, string? appUserId, CancellationToken cancelationToken)
    {
        if (logType == (int)LogType.Private && string.IsNullOrEmpty(appUserId))
        {
            return [];
        }

        var combatLogs = _context.Set<CombatLog>()
            .AsNoTracking();

        IQueryable<CombatLog> filter = combatLogs;
        if (logType == (int)LogType.Public)
        {
            filter = combatLogs.Where(cl => cl.LogType == logType && cl.GameVersion == gameVersion);
        }
        else if (logType == (int)LogType.Private)
        {
            filter = combatLogs.Where(cl => cl.LogType == logType && cl.GameVersion == gameVersion && cl.AppUserId == appUserId);
        }

        var result = await filter
            .ToListAsync(cancelationToken);

        return result.Count != 0 ? result : [];
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(CONTEXT_TIMEOUT_MINUTES));

        await _context.Set<CombatLog>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
