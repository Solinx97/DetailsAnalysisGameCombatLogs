using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerPositionRepository(CombatParserContextOne context) : ICombatPlayerPositionRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatPlayerPosition>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<CombatPlayerPosition>()
                    .AsNoTracking()
                    .Where(x => x.CombatPlayerId == combatPlayerId)
                    .ToListAsync(cancellationToken);

        return data.Count != 0 ? data : [];
    }
}
