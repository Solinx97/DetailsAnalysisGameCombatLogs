using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Dashboard;

internal class DashboardRepository(CombatParserContextOne context) : IDashboardRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Domain.Entities.Dashboard.Dashboard[]> GetAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var dashboards = await _context.Set<Combat>()
            .AsNoTracking()
            .Where(x => x.CombatLogId == combatLogId)
            .Join(_context.Set<CombatPlayer>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        u.Id,
                        u.Player.Username,
                        u.DamageDone,
                        u.HealDone,
                        Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(EF.Functions, x.StartDate, x.FinishDate)
                    })
            .GroupJoin(_context.Set<CombatPlayerDeath>(),
                    x => x.Id,
                    u => u.CombatPlayerId,
                    (x, u) => new
                    {
                        x.Username,
                        x.DamageDone,
                        x.HealDone,
                        x.Duration,
                        Deaths = u.Count()
                    })
            .GroupBy(x => x.Username)
            .Select(g => new Domain.Entities.Dashboard.Dashboard(
                    g.Key,
                    Math.Round((double)g.Sum(x => (long)x.DamageDone) / g.Sum(x => x.Duration), 2),
                    Math.Round((double)g.Sum(x => (long)x.HealDone) / g.Sum(x => x.Duration), 2),
                    g.Sum(x => x.Deaths)))
            .ToArrayAsync(cancellationToken);

        return dashboards;
    }
}
