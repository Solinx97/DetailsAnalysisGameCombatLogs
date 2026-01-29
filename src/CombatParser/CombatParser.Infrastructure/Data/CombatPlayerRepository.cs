using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerRepository(CombatParserContextOne context) : GenericRepository<Combat, int>(context), ICombatPlayerRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayers = await _context.Set<CombatPlayer>()
            .Where(c => c.CombatId == combatId)
            .Include(c => c.Player)
            .Include(c => c.Stats)
            .Include(c => c.Score)
            .ToListAsync(cancellationToken);

        return combatPlayers;
    }
}
