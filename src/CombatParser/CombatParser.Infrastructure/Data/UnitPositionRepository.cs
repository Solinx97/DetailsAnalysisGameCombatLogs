using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitPositionRepository(CombatParserContextOne context) : IUnitPositionRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IDictionary<string, IEnumerable<UnitPosition>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitPosition>()
                    .AsNoTracking()
                    .Where(x => x.CombatId == combatId)
                    .GroupBy(x => x.GameId)
                    .ToDictionaryAsync(x => x.Key, x => x.Select(y => y), cancellationToken);

        return data.Count != 0 ? data : [];
    }
}
