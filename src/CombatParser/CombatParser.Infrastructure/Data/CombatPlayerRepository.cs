using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerRepository(CombatParserContextOne context) : ICombatPlayerRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var combatPlayers = await _context.Set<CombatPlayer>()
            .AsNoTracking()
            .Where(c => c.CombatId == combatId)
            .Include(c => c.Player)
            .Include(c => c.Score)
            .ToListAsync(cancellationToken);

        return combatPlayers;
    }

    public async Task<CombatPlayer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var combatPlayer = await _context.Set<CombatPlayer>()
            .AsNoTracking()
            .Include(c => c.Player)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return combatPlayer;
    }
}
