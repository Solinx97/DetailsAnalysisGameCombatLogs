using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitRepository(CombatParserContextOne context) : IUnitRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<Unit>> GetAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<Unit>()
                    .Where(x => x.CombatId == combatId)
                    .Include(x => x.UnitPositions
                        .OrderBy(x => x.Time)
                     )
                    .Include(x => x.UnitHealthes
                        .OrderBy(x => x.Time)
                     )
                    .Include(x => x.UnitCasts
                        .OrderBy(x => x.Time)
                     )
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UniqueUnitName>> GetUniqueNamesAsync(int combatLogId, string bossName, CancellationToken cancellationToken)
    {
        var data = await _context.Set<Unit>()
                    .Where(x => x.Combat.CombatLogId == combatLogId
                            && (string.IsNullOrWhiteSpace(bossName) || x.Combat.Boss.Name == bossName))
                    .AsNoTracking()
                    .Select(x => new UniqueUnitName
                    {
                        Name = x.Name,
                        Type = x.Type
                    })
                    .Distinct()
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UnitPosition>> GetPositionsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitPosition>()
                    .Where(x => x.UnitId == combatUnitId)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UnitCast>> GetCastsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitCast>()
                    .Where(x => x.UnitId == combatUnitId)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IDictionary<string, List<UnitHealth>>> GetUnitsHealthAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitHealth>()
                    .Where(x => x.Unit.CombatId == combatId)
                    .AsNoTracking()
                    .GroupBy(x => x.Unit.GameId)
                    .ToDictionaryAsync(
                        x => x.Key,
                        x => x
                            .OrderBy(y => y.Time)
                            .ToList(),
                        cancellationToken);

        return data;
    }

    public async Task<List<UnitHealth>> GetUnitsHealthByIntervalAsync(string unitId, string from, string to, CancellationToken cancellationToken)
    {
        var query = _context.Set<UnitHealth>()
                    .AsQueryable();

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            var fromTime = TimeSpan.Parse(from);
            var toTime = TimeSpan.Parse(to);

            query = query.Where(x => x.UnitId == unitId && x.Time >= fromTime && x.Time <= toTime);
        }
        else
        {
            query = query.Where(x => x.UnitId == unitId);
        }

        var data = await query
                    .AsNoTracking()
                    .OrderBy(x => x.Time)
                    .Reverse()
                    .ToListAsync(cancellationToken);

        return data;
    }
}
