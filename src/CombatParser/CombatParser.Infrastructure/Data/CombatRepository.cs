using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Extensions;
using CombatParser.Infrastructure.Persistent;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatRepository(CombatParserContextOne context) : ICombatRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task AddBulkAsync(Combat combat, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        await _context.BulkInsertAsync([combat], new BulkConfig
        {
            SetOutputIdentity = true,
            PreserveInsertOrder = true
        }, cancellationToken: cancellationToken);

        var players = await _context.BulkInsertCombatPlayersAsync(combat.Id, combat.CombatPlayers, cancellationToken);

        await _context.BulkInsertCombatPlayerStatsAsync(players, cancellationToken);
        await _context.BulkInsertCombatPlayerScoresAsync(combat.BossId, players, cancellationToken);

        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.PreAuras, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.Auras, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.Casts, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageDones, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageDoneGenerals, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.HealDones, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.HealDoneGenerals, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageTakens, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageTakenGenerals, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.ResourceRecoveries, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.ResourceRecoveryGenerals, cancellationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.CombatPlayerDeathes, cancellationToken);
        await _context.BulkInsertCombatPlayerPositionsAsync(players, p => p.CombatPlayerPositions, cancellationToken);

        if (combat.IsWin)
        {
            await _context.BulkUpdateBestSpecializationScoreAsync(combat.BossId, players, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<IEnumerable<Combat>> GetByCombatLogIdAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var combats = await _context.Set<Combat>()
            .Where(c => c.CombatLogId == combatLogId)
            .Include(c => c.Boss)
            .ToListAsync(cancellationToken);

        return combats;
    }

    public async Task<Combat?> GetByIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var combat = await _context.Set<Combat>()
            .Include(c => c.Boss)
            .FirstOrDefaultAsync(c => c.Id == combatId, cancellationToken);

        return combat;
    }
}