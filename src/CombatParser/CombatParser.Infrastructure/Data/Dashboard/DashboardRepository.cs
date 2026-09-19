using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.Dashboard;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Dashboard;

internal class DashboardRepository(CombatParserContextOne context) : IDashboardRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamagePerSecondAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, combatId, unitName);

        var dashboardItems = await query
             .Join(_context.Set<UnitHealth>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        BossName = x.Combat.Boss.Name,
                        x.Name,
                        x.UnitInfo.DamageDone,
                        Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(EF.Functions, x.Combat.StartDate, x.Combat.FinishDate)
                    })
            .Where(x => x.DamageDone > 0)
            .AsNoTracking()
            .GroupBy(x => string.IsNullOrEmpty(unitName) 
                ? x.Name 
                : x.BossName)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    (long)Math.Round((double)g.Sum(x => x.DamageDone) / g.Sum(x => x.Duration), 2)))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString()))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetHealPerSecondAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, combatId, unitName);

        var dashboardItems = await query
             .Join(_context.Set<UnitHealth>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        BossName = x.Combat.Boss.Name,
                        x.Name,
                        x.UnitInfo.HealDone,
                        Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(EF.Functions, x.Combat.StartDate, x.Combat.FinishDate)
                    })
            .Where(x => x.HealDone > 0)
            .AsNoTracking()
            .GroupBy(x => string.IsNullOrEmpty(unitName)
                ? x.Name
                : x.BossName)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    (long)Math.Round((double)g.Sum(x => x.HealDone) / g.Sum(x => x.Duration), 2)))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString()))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamageSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, combatId, unitName);

        var dashboardItems = await query
            .Join(_context.Set<DamageDone>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        u.Spell,
                        u.Value
                    })
            .GroupBy(x => x.Spell)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    g.Sum(x => x.Value)))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString()))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(1, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetHealSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, combatId, unitName);

        var dashboardItems = await query
            .Join(_context.Set<HealDone>(),
                x => x.Id,
                y => y.UnitId,
                (x, y) => new
                {
                    y.Spell,
                    y.Value
                })
            .GroupBy(x => x.Spell)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    g.Sum(x => x.Value)))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString()))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(1, items);
        return dashboard;
    }

    private IQueryable<Unit> GetQuery(int combatLogId, int combatId, string unitName)
    {
        var query = _context.Set<Combat>()
            .AsQueryable();
        if (combatId > 0)
        {
            query = query.Where(x => x.CombatLogId == combatLogId && x.Id == combatId);
        }
        else
        {
            query = query.Where(x => x.CombatLogId == combatLogId);
        }

        var queryUnits = query
            .Join(_context.Set<Unit>(),
                x => x.Id,
                u => u.CombatId,
                (x, u) => u);
        if (!string.IsNullOrEmpty(unitName))
        {
            queryUnits = queryUnits.Where(x => x.Name == unitName);
        }

        return queryUnits;
    }
}
