using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
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

        var auras = combat.CombatAuras.Select(p =>
        {
            p.SetCombatId(combat.Id);

            return p;
        }).ToList();
        await _context.BulkInsertAsync(auras, cancellationToken: cancelationToken);

        var players = combat.CombatPlayers.Select(p => 
        {
            p.SetCombatId(combat.Id);

            return p;
        }).ToList();
        await _context.BulkInsertAsync(players, new BulkConfig
        {
            PreserveInsertOrder = true,
            SetOutputIdentity = true
        }, cancellationToken: cancelationToken);

        var stats = combat.CombatPlayers.Select(p =>
        {
            var stats = p.Stats;
            stats.SetCombatPlayerId(p.Id);

            return stats;
        }).ToList();
        await _context.BulkInsertAsync(stats, cancellationToken: cancelationToken);

        var scores = combat.CombatPlayers.Select(p =>
        {
            var score = p.Score;
            score?.SetCombatPlayerId(p.Id);

            return score;
        }).Where(s => s != null).ToList();
        await _context.BulkInsertAsync(scores, cancellationToken: cancelationToken);

        var damageDones = players.SelectMany(p =>
        {
            var damageDones = p.DamageDones.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(damageDones, cancellationToken: cancelationToken);

        var damageDoneGenerals = players.SelectMany(p =>
        {
            var damageDones = p.DamageDoneGenerals.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(damageDones, cancellationToken: cancelationToken);

        var healDones = players.SelectMany(p =>
        {
            var damageDones = p.HealDones.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(healDones, cancellationToken: cancelationToken);

        var healDoneGenerals = players.SelectMany(p =>
        {
            var damageDones = p.HealDoneGenerals.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(healDoneGenerals, cancellationToken: cancelationToken);

        var damageTakens = players.SelectMany(p =>
        {
            var damageDones = p.DamageTakens.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(damageTakens, cancellationToken: cancelationToken);

        var damageTakenGenerals = players.SelectMany(p =>
        {
            var damageDones = p.DamageTakenGenerals.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(damageTakenGenerals, cancellationToken: cancelationToken);

        var resourceRecoveries = players.SelectMany(p =>
        {
            var damageDones = p.ResourceRecoveries.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(resourceRecoveries, cancellationToken: cancelationToken);

        var resourceRecoveryGenerals = players.SelectMany(p =>
        {
            var damageDones = p.ResourceRecoveryGenerals.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(resourceRecoveryGenerals, cancellationToken: cancelationToken);

        var deathes = players.SelectMany(p =>
        {
            var damageDones = p.CombatPlayerDeathes.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(deathes, cancellationToken: cancelationToken);

        var positions = players.SelectMany(p =>
        {
            var damageDones = p.CombatPlayerPositions.Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);

                return dd;
            });

            return damageDones;
        });
        await _context.BulkInsertAsync(positions, cancellationToken: cancelationToken);

        await transaction.CommitAsync(cancelationToken);
    }
}