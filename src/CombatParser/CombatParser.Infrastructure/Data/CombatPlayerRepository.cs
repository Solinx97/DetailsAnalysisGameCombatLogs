using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Domain.Interfaces;
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

    public async Task<IPlayerStats?> GetPlayerStatsAsync(int combatPlayerId, int gameVersion, CancellationToken cancellationToken)
    {
        return gameVersion switch
        {
            0 => await _context.Set<WoWMoPClassicPlayerStats>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.CombatPlayerId == combatPlayerId,
                    cancellationToken),

            1 => await _context.Set<WoWMidnightPlayerStats>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.CombatPlayerId == combatPlayerId,
                    cancellationToken),

            _ => throw new ArgumentOutOfRangeException(nameof(gameVersion))
        };
    }
}
