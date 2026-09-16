using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Domain.Enums;
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
            .Include(c => c.Player)
            .Include(c => c.Score)
            .Include(x => x.Unit)
                .ThenInclude(x => x.UnitInfo)
            .Where(c => c.CombatId == combatId)
            .Select(x => new
            {
                Player = x,
                DeathCount = _context.Set<UnitHealth>()
                    .Where(h => h.UnitId == x.UnitId)
                    .Count(h => h.Status == (int)UnitHealthStatus.Dead)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var x in combatPlayers)
        {
            x.Player.SetDeathCount(x.DeathCount);
        }

        return [.. combatPlayers.Select(x => x.Player)];
    }

    public async Task<CombatPlayer?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var combatPlayer = await _context.Set<CombatPlayer>()
            .Include(x => x.Unit)
                .ThenInclude(x => x.UnitInfo)
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
