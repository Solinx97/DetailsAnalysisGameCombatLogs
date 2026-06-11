using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerPositionRepository(CombatParserContextOne context)
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatPlayerPosition>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<CombatPlayerPosition>()
                    .AsNoTracking()
                    .Where(x => x.CombatPlayerId == combatId)
                    .ToListAsync(cancellationToken);

        return data.Count != 0 ? data : [];
    }
}
