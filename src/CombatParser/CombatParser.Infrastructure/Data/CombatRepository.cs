using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Extensions;
using CombatParser.Infrastructure.Persistence;
using EFCore.BulkExtensions;

namespace CombatParser.Infrastructure.Data;

internal class CombatRepository(CombatParserContextOne context) : GenericRepository<Combat, int>(context), ICombatRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task AddBulkAsync(Combat combat, CancellationToken cancelationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancelationToken);

        await _context.BulkInsertAsync([combat], new BulkConfig
        {
            SetOutputIdentity = true
        }, cancellationToken: cancelationToken);

        var players = await _context.BulkInsertCombatPlayersAsync(combat.Id, combat.CombatPlayers, cancelationToken);
        await _context.BulkInsertCombatAurasAsync(combat.Id, combat.CombatAuras, cancelationToken);

        await _context.BulkInsertCombatPlayerStatsAsync(players, cancelationToken);
        await _context.BulkInsertCombatPlayerScoresAsync(players, cancelationToken);

        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageDones, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageDoneGenerals, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.HealDones, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.HealDoneGenerals, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageTakens, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.DamageTakenGenerals, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.ResourceRecoveries, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.ResourceRecoveryGenerals, cancelationToken);
        await _context.BulkInsertCombatPlayerDataAsync(players, p => p.CombatPlayerPositions, cancelationToken);

        await transaction.CommitAsync(cancelationToken);
    }
}