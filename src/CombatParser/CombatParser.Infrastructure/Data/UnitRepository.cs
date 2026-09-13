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
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UnitPosition>> GetPositionsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitPosition>()
                    .AsNoTracking()
                    .Where(x => x.CombatUnitId == combatUnitId)
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UnitCast>> GetCastsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitCast>()
                    .AsNoTracking()
                    .Where(x => x.CombatUnitId == combatUnitId)
                    .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IDictionary<string, List<UnitHealth>>> GetHealthesAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<Unit>()
                    .Join(_context.Set<UnitHealth>(),
                        x => x.Id,
                        y => y.CombatUnitId,
                        (x, y) => new
                        {
                            CombatId = x.CombatId,
                            GameId = x.GameId,
                            Time = y.Time,
                            Health = y
                        })
                    .AsNoTracking()
                    .Where(x => x.CombatId == combatId)
                    .GroupBy(x => x.GameId)
                    .ToDictionaryAsync(
                        x => x.Key,
                        x => x
                            .OrderBy(y => y.Time)
                            .Select(y => y.Health)
                            .ToList(),
                        cancellationToken);

        return data;
    }
}
