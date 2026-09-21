using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.Dashboard;
using CombatParser.Domain.Enums;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Dashboard;

internal class DashboardRepository(CombatParserContextOne context) : IDashboardRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamageAsync(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var generalQuery = GetQuery(combatLogId, bossName, combatId);
        var queryValues = generalQuery
                .SelectMany(x => x.DamageDones
                                .Where(x => (string.IsNullOrEmpty(creatorName) || x.Unit.Name == creatorName)
                                    && (string.IsNullOrEmpty(targetName) || x.Target.Name == targetName)),
                            (x, y) => new DashboardQuery<DamageDone>
                            {
                                Unit = x,
                                Value = y
                            });

        var dashboardItems = await GatDashboardItemsQuery(queryValues, bossName, creatorName, targetName, valueType, cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetHealAsync(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        var generalQuery = GetQuery(combatLogId, bossName, combatId);
        var queryValues = generalQuery
                .SelectMany(x => x.HealDones
                                .Where(x => (string.IsNullOrEmpty(creatorName) || x.Unit.Name == creatorName)
                                    && (string.IsNullOrEmpty(targetName) || x.Target.Name == targetName)),
                    (x, y) => new DashboardQuery<HealDone>
                    {
                        Unit = x,
                        Value = y
                    });

        if (!string.IsNullOrEmpty(targetName))
        {
            queryValues = queryValues.Where(x => x.Value.Target.Name == targetName);
        }

        var dashboardItems = await GatDashboardItemsQuery(queryValues, bossName, creatorName, targetName, valueType, cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(0, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetDamageSpellsAsync(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, bossName, combatId);

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
                    g.Sum(x => (long)x.Value),
                    g.Select(x => x.Type).First()))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(1, items);
        return dashboard;
    }

    public async Task<Domain.Entities.Dashboard.Dashboard> GetHealSpellsAsync(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken)
    {
        var query = GetQuery(combatLogId, bossName, combatId);

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
                    g.Sum(x => (long)x.Value),
                    g.Select(x => x.Type).First()))
            .ToListAsync(cancellationToken);

        var items = dashboardItems
            .OrderByDescending(x => x.Value)
            .Select(x => new DashboardItem(x.ValueName, x.Value.ToString(), x.UnitType))
            .ToList();
        var dashboard = new Domain.Entities.Dashboard.Dashboard(1, items);
        return dashboard;
    }

    private IQueryable<Unit> GetQuery(int combatLogId, string bossName, int combatId)
    {
        var query = _context.Set<Combat>()
            .Where(x => x.CombatLogId == combatLogId
                        && (combatId <= 0 || x.Id == combatId)
                        && (string.IsNullOrEmpty(bossName) || x.Boss.Name == bossName))
            .Join(_context.Set<Unit>(),
                x => x.Id,
                u => u.CombatId,
                (x, u) => u);

        return query;
    }

    private static async Task<List<DashboardItemNumber>> GatDashboardItemsQuery<TModel>(IQueryable<DashboardQuery<TModel>> queryValues, string bossName, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
        where TModel : class, IGeneralEntity, ICombatUnitRefs, IUnitTargetRefs
    {
        var query = queryValues
                .GroupBy(x => new
                {
                    x.Unit.Id,
                    x.Unit.Name,
                    CreatorName = x.Value.Unit.Name,
                    TargetName = x.Value.Target.Name,
                    x.Unit.Type,
                    BossName = x.Unit.Combat.Boss.Name,
                    x.Unit.Combat.StartDate,
                    x.Unit.Combat.FinishDate
                })
                .Select(x => new DashboardValue
                {
                    CreatorName = x.Key.CreatorName,
                    TargetName = x.Key.TargetName,

                    Name = x.Key.Name,
                    BossName = x.Key.BossName,
                    UnitType = x.Key.Type,

                    Value = x.Sum(y => y.Value.Value),

                    Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(
                            EF.Functions,
                            x.Key.StartDate,
                            x.Key.FinishDate)
                })
                .Where(x => x.Value > 0 && x.Duration > 0)
                .AsNoTracking();

        var dashboardItems = await AppValueTypeAsync(query, bossName, creatorName, targetName, valueType, cancellationToken);

        return dashboardItems;
    }

    private static async Task<List<DashboardItemNumber>> AppValueTypeAsync(IQueryable<DashboardValue> query, string bossName, string creatorName, string targetName, int valueType, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(DashboardValueType), valueType))
        {
            throw new ArgumentOutOfRangeException();
        }

        var valueTypeEnum = (DashboardValueType)valueType;

        var grouping = query.GroupBy(x =>
            !string.IsNullOrEmpty(creatorName) ? x.TargetName :
            !string.IsNullOrEmpty(targetName) ? x.CreatorName :
            x.Name);

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
