using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.Dashboard;
using CombatParser.Domain.Enums;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Dashboard;

internal class DashboardRepository(CombatParserContextOne context) : IDashboardRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamageAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var generalQuery = GetQuery(combatLogId, combatId, unitName);
        var query = generalQuery
            .Join(
                _context.Set<UnitHealth>(),
                x => x.Id,
                u => u.UnitId,
                (x, u) => new DashboardValue
                {
                    BossName = x.Combat.Boss.Name,
                    Name = x.Name,
                    UnitType = x.Type,
                    Value = x.UnitInfo.DamageDone,
                    Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(
                        EF.Functions,
                        x.Combat.StartDate,
                        x.Combat.FinishDate)
                })
                .Where(x => x.Value > 0 && x.Duration > 0)
                .AsNoTracking();

        var dashboardItems = await AppValueTypeAsync(query, unitName, valueType, cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetHealAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var generalQuery = GetQuery(combatLogId, combatId, unitName);
        var query = generalQuery
             .Join(_context.Set<UnitHealth>(),
                    x => x.Id,
                    u => u.UnitId,
                    (x, u) => new DashboardValue
                    {
                        Name = x.Name,
                        BossName = x.Combat.Boss.Name,
                        UnitType = x.Type,
                        Value = x.UnitInfo.HealDone,
                        Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(
                            EF.Functions,
                            x.Combat.StartDate,
                            x.Combat.FinishDate)
                    })
            .Where(x => x.Value > 0 && x.Duration > 0)
            .AsNoTracking();

        var dashboardItems = await AppValueTypeAsync(query, unitName, valueType, cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamageTakenAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken)
    {
        var generalQuery = GetQuery(combatLogId, combatId, unitName);
        var query = generalQuery
            .Join(
                _context.Set<UnitHealth>(),
                x => x.Id,
                u => u.UnitId,
                (x, u) => new DashboardValue
                {
                    BossName = x.Combat.Boss.Name,
                    Name = x.Name,
                    UnitType = x.Type,
                    Value = x.UnitInfo.DamageTaken,
                    Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(
                        EF.Functions,
                        x.Combat.StartDate,
                        x.Combat.FinishDate)
                })
                .Where(x => x.Value > 0 && x.Duration > 0)
                .AsNoTracking();

        var dashboardItems = await AppValueTypeAsync(query, unitName, valueType, cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
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
                        x.Type,
                        u.Spell,
                        u.Value
                    })
            .GroupBy(x => x.Spell)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    g.Sum(x => x.Value),
                    g.Select(x => x.Type).First()))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
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
                    x.Type,
                    y.Spell,
                    y.Value
                })
            .GroupBy(x => x.Spell)
            .Select(g => new DashboardItemNumber(
                    g.Key,
                    g.Sum(x => x.Value),
                    g.Select(x => x.Type).First()))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
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

    private static async Task<List<DashboardItemNumber>> AppValueTypeAsync(IQueryable<DashboardValue> query, string unitName, int valueType, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(DashboardValueType), valueType))
        {
            throw new ArgumentOutOfRangeException();
        }

        var valueTypeEnum = (DashboardValueType)valueType;

        var grouping = query.GroupBy(x => string.IsNullOrEmpty(unitName)
                    ? x.Name
                    : x.BossName);

        var result = valueTypeEnum switch
        {
            DashboardValueType.Value => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        g.Sum(x => x.Value),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.AverageValue => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        (long)Math.Round(g.Average(x => x.Value), 2),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.MaxValue => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        g.Max(x => x.Value),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.MinValue => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        g.Min(x => x.Value),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.ValuePerSecond => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        (long)Math.Round((double)g.Sum(x => x.Value) / g.Sum(x => x.Duration), 2),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.AverageValuePerSecond => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        (long)Math.Round((double)g.Average(x => (double)x.Value / x.Duration), 2),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.MaxValuePerSecond => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        (long)Math.Round((double)g.Max(x => (double)x.Value / x.Duration), 2),
                                        g.Select(x => x.UnitType).First())),
            DashboardValueType.MinValuePerSecond => grouping.Select(g => new DashboardItemNumber(
                                        g.Key,
                                        (long)Math.Round((double)g.Min(x => (double)x.Value / x.Duration), 2),
                                        g.Select(x => x.UnitType).First())),
            _ => throw new ArgumentOutOfRangeException(),
        };

        return await result.ToListAsync(cancellationToken);
    }
}
