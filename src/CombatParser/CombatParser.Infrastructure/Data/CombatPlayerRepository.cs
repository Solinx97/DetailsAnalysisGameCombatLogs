using CombatParser.Domain.Consts;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
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

    public async Task<IEnumerable<string>> GetUniquePlayerNames(int combatLogId, CancellationToken cancellationToken)
    {
        var combatPlayerNames = await _context.Set<CombatPlayer>()
            .Where(c => c.Combat.CombatLogId == combatLogId)
            .AsNoTracking()
            .Select(x => x.Player.Username)
            .Distinct()
            .ToListAsync(cancellationToken);

        return combatPlayerNames;
    }

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

    public async Task<int> GetPlayerDeathCountAsync(string unitId, CancellationToken cancellationToken)
    {
        var countDeath = await _context.Set<UnitHealth>()
            .Where(x =>
                x.UnitId == unitId &&
                x.Status == (int)UnitHealthStatus.Dead)
            .CountAsync(cancellationToken);

        return countDeath;
    }

    public async Task<TimeSpan?> GetWhenPlayerDeathAsync(string unitId, int skipCount, CancellationToken cancellationToken)
    {
        var whenDied = await _context.Set<UnitHealth>()
            .Where(x =>
                x.UnitId == unitId &&
                x.Status == (int)UnitHealthStatus.Dead)
            .OrderBy(x => x.Time)
            .Select(x => (TimeSpan?)x.Time)
            .Skip(skipCount)
            .Take(1)
            .FirstOrDefaultAsync(cancellationToken);

        return whenDied;
    }

    public async Task<List<CombatPlayerDeath>> GetPlayerDeathAsync(string unitId, string whenDied, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(whenDied))
        {
            return [];
        }

        var whenDiedTime = TimeSpan.Parse(whenDied);
        var fromTime = whenDiedTime - PlayerDeathValue.IntervalBeforeDied;

        var damage = await _context.Set<DamageDone>()
            .Where(x =>
                x.TargetId == unitId &&
                x.Time >= fromTime &&
                x.Time <= whenDiedTime)
            .AsNoTracking()
            .Select(x => new CombatPlayerDeath(
                x.Time,
                x.Unit.Name,
                x.Spell,
                x.Value,
                0,
                0,
                0,
                x.UnitId))
        .ToListAsync(cancellationToken);

        var heal = await _context.Set<HealDone>()
            .Where(x =>
                x.TargetId == unitId &&
                x.Time >= fromTime &&
                x.Time <= whenDiedTime)
            .AsNoTracking()
            .Select(x => new CombatPlayerDeath(
                x.Time,
                x.Unit.Name,
                x.Spell,
                x.Value,
                0,
                0,
                0,
                x.UnitId))
        .ToListAsync(cancellationToken);

        var data = damage
            .Concat(heal)
            .OrderBy(x => x.Time)
            .Reverse()
            .ToList();

        return data;
    }
}
