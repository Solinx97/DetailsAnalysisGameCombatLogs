using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data.Dashboard;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Enums;
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
            .Join(_context.Set<Unit>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        Unit = u
                    })
            .Join(_context.Set<UnitInfo>(),
                    x => x.Unit.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        x.Unit,
                        UnitInfo = u,
                    })
             .Join(_context.Set<UnitHealth>(),
                    x => x.Unit.Id,
                    u => u.UnitId,
                    (x, u) => new
                    {
                        x.Unit.Name,
                        x.Unit.Type,
                        x.UnitInfo.DamageDone,
                        x.UnitInfo.HealDone,
                        Status = u.Status,
                        Duration = SqlServerDbFunctionsExtensions.DateDiffSecond(EF.Functions, x.Unit.Combat.StartDate, x.Unit.Combat.FinishDate)
                    })
            .Where(x => x.DamageDone > 0 || x.HealDone > 0)
            .GroupBy(x => x.Name)
            .Select(g => new Domain.Entities.Dashboard.Dashboard(
                    g.Key,
                    g.Select(x => x.Type).First(),
                    Math.Round((double)g.Sum(x => (long)x.DamageDone) / g.Sum(x => x.Duration), 2),
                    Math.Round((double)g.Sum(x => (long)x.HealDone) / g.Sum(x => x.Duration), 2),
                    g.Count(x => x.Status == (int)UnitHealthStatus.Dead)))
            .ToArrayAsync(cancellationToken);

        return dashboards;
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

    public async Task<Dictionary<string, long>> GetHealSpellsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var spells = await _context.Set<Combat>()
            .AsNoTracking()
            .Where(x => x.CombatLogId == combatLogId)
            .Join(
                _context.Set<Unit>(),
                combat => combat.Id,
                unit => unit.CombatId,
                (combat, unit) => unit.Id)
            .Join(
                _context.Set<HealDone>(),
                unitId => unitId,
                heal => heal.UnitId,
                (unitId, heal) => new
                {
                    heal.Spell,
                    heal.Value
                })
            .GroupBy(x => x.Spell)
            .ToDictionaryAsync(
                x => x.Key,
                x => x.Sum(y => (long)y.Value),
                cancellationToken);

        return spells;
    }
}
