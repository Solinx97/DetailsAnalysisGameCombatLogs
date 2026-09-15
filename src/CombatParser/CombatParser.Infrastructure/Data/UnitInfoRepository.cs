using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Enums;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitInfoRepository<TModel>(CombatParserContextOne context) : IUnitInfoRepository<TModel>
    where TModel : class, ICombatUnitRefs
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByUnitIdAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<DamageDoneGeneral>> GetDamageByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var duration = await GetDurationAsync(combatId, cancellationToken);

        var data = await _context.Set<DamageDone>()
            .AsNoTracking()
            .Where(x =>
                x.UnitId == unitId &&
                x.Unit.CombatId == combatId)
             .GroupBy(x => x.GameSpellId)
             .Select(x => new
             {
                 GameSpellId = x.Key,
                 Spell = x.Select(y => y.Spell).First(),
                 Value = x.Sum(y => y.Value),
                 CritCount = x.Count(y =>
                     y.ModificationType == (int)ModificationType.Crit),
                 ModifiedCount = x.Count(y =>
                     y.ModificationType != (int)ModificationType.Crit &&
                     y.ModificationType != (int)ModificationType.Normal &&
                     y.ModificationType != (int)ModificationType.Absorb),
                 Count = x.Count(),
                 Min = x.Min(y => y.Value),
                 Max = x.Max(y => y.Value),
                 Average = x.Average(y => y.Value)
             })
            .ToListAsync(cancellationToken);

        var result = data
            .Select(x => DamageDoneGeneral.Create(
                x.GameSpellId,
                x.Spell,
                x.Value,
                x.Value / duration.TotalSeconds,
                x.CritCount,
                x.ModifiedCount,
                x.Count,
                x.Min,
                x.Max,
                x.Average))
            .OrderByDescending(x => x.Value)
            .ToList();

        return result;
    }

    public async Task<IEnumerable<DamageDoneGeneral>> GetDamageTakenByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var duration = await GetDurationAsync(combatId, cancellationToken);

        var data = await _context.Set<DamageDone>()
            .AsNoTracking()
            .Where(x =>
                x.TargetId == unitId &&
                x.Unit.CombatId == combatId)
             .GroupBy(x => x.GameSpellId)
             .Select(x => new
             {
                 GameSpellId = x.Key,
                 Spell = x.Select(y => y.Spell).First(),
                 Value = x.Sum(y => y.Value),
                 CritCount = x.Count(y =>
                     y.ModificationType == (int)ModificationType.Crit),
                 ModifiedCount = x.Count(y =>
                     y.ModificationType != (int)ModificationType.Crit &&
                     y.ModificationType != (int)ModificationType.Normal &&
                     y.ModificationType != (int)ModificationType.Absorb),
                 Count = x.Count(),
                 Min = x.Min(y => y.Value),
                 Max = x.Max(y => y.Value),
                 Average = x.Average(y => y.Value)
             })
            .ToListAsync(cancellationToken);

        var result = data
            .Select(x => DamageDoneGeneral.Create(
                x.GameSpellId,
                x.Spell,
                x.Value,
                x.Value / duration.TotalSeconds,
                x.CritCount,
                x.ModifiedCount,
                x.Count,
                x.Min,
                x.Max,
                x.Average))
            .OrderByDescending(x => x.Value)
            .ToList();

        return result;
    }

    public async Task<IEnumerable<HealDoneGeneral>> GetHealByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var duration = await GetDurationAsync(combatId, cancellationToken);

        var data = await _context.Set<HealDone>()
            .AsNoTracking()
            .Where(x =>
                x.UnitId == unitId &&
                x.Unit.CombatId == combatId)
             .GroupBy(x => x.GameSpellId)
             .Select(x => new
             {
                 GameSpellId = x.Key,
                 Spell = x.Select(y => y.Spell).First(),
                 Value = x.Sum(y => y.Value),
                 CritCount = x.Count(y =>
                     y.ModificationType == (int)ModificationType.Crit),
                 Count = x.Count(),
                 Min = x.Min(y => y.Value),
                 Max = x.Max(y => y.Value),
                 Average = x.Average(y => y.Value)
             })
            .ToListAsync(cancellationToken);

        var result = data
            .Select(x => HealDoneGeneral.Create(
                x.GameSpellId,
                x.Spell,
                x.Value,
                x.Value / duration.TotalSeconds,
                x.CritCount,
                x.Count,
                x.Min,
                x.Max,
                x.Average))
            .OrderByDescending(x => x.Value)
            .ToList();

        return result;
    }

    public async Task<IEnumerable<ResourceRecoveryGeneral>> GetResourcesByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken)
    {
        var duration = await GetDurationAsync(combatId, cancellationToken);

        var data = await _context.Set<ResourceRecovery>()
            .AsNoTracking()
            .Where(x =>
                x.UnitId == unitId &&
                x.Unit.CombatId == combatId)
             .GroupBy(x => x.GameSpellId)
             .Select(x => new
             {
                 GameSpellId = x.Key,
                 Spell = x.Select(y => y.Spell).First(),
                 Value = x.Sum(y => y.Value),
                 Count = x.Count(),
                 Min = x.Min(y => y.Value),
                 Max = x.Max(y => y.Value),
                 Average = x.Average(y => y.Value)
             })
            .ToListAsync(cancellationToken);

        var result = data
            .Select(x => ResourceRecoveryGeneral.Create(
                x.GameSpellId,
                x.Spell,
                x.Value,
                x.Value / duration.TotalSeconds,
                x.Count,
                x.Min,
                x.Max,
                x.Average))
            .OrderByDescending(x => x.Value)
            .ToList();

        return result;
    }

    public async Task<TModel?> GetFirstByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .SingleOrDefaultAsync(cancellationToken);

        return data;
    }

    private async Task<TimeSpan> GetDurationAsync(int combatId, CancellationToken cancellationToken)
    {
        var duration = await _context.Set<Combat>()
            .Where(x => x.Id == combatId)
            .Select(x => x.FinishDate - x.StartDate)
            .FirstAsync(cancellationToken);

        return duration;
    }
}
