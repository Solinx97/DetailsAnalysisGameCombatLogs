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
        //var dashboards = await _context.Set<Combat>()
        //    .AsNoTracking()
        //    .Where(x => x.CombatLogId == combatLogId)
        //    .Join(_context.Set<Unit>(),
        //            x => x.Id,
        //            u => u.CombatId,
        //            (x, u) => new
        //            {
        //                Combat = x,
        //                Unit = u
        //            })
        //    .Join(_context.Set<UnitInfo>(),
        //            x => x.Unit.Id,
        //            u => u.Id,
        //            (x, u) => new
        //            {
        //                u.Id,
        //                x.Name,
        //                u.DamageDone,
        //                u.HealDone,
        //                Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(EF.Functions, x.Combat.StartDate, x.Combat.FinishDate)
        //            })
        //    .GroupBy(x => x.Username)
        //    .Select(g => new Domain.Entities.Dashboard.Dashboard(
        //            g.Key,
        //            Math.Round((double)g.Sum(x => (long)x.DamageDone) / g.Sum(x => x.Duration), 2),
        //            Math.Round((double)g.Sum(x => (long)x.HealDone) / g.Sum(x => x.Duration), 2),
        //            0))
        //    .ToArrayAsync(cancellationToken);

        //return dashboards;
        return [];
    }

    public async Task<Dictionary<string, int>> GetDamageSpellsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var spells = await _context.Set<Combat>()
            .AsNoTracking()
            .Where(x => x.CombatLogId == combatLogId)
            .Join(_context.Set<Unit>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        u.Id,
                    })
            .Join(_context.Set<DamageDone>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        u.Spell,
                        u.Value
                    })
            .GroupBy(x => x.Spell)
            .ToDictionaryAsync(x => x.Key, g => g.Sum(x => x.Value), cancellationToken);

        return spells;
    }

    public async Task<Dictionary<string, int>> GetHealSpellsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var spells = await _context.Set<Combat>()
            .AsNoTracking()
            .Where(x => x.CombatLogId == combatLogId)
            .Join(_context.Set<Unit>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        u.Id,
                    })
            .Join(_context.Set<HealDone>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        u.Spell,
                        u.Value
                    })
            .GroupBy(x => x.Spell)
            .ToDictionaryAsync(x => x.Key, g => g.Sum(x => x.Value), cancellationToken);

        return spells;
    }
}
