using CombatParser.Domain.Entities;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistence;
using EFCore.BulkExtensions;

namespace CombatParser.Infrastructure.Extensions;

internal static class CombatParserContextOneExtension
{
    public static async Task BulkInsertCombatPlayerDataAsync<TModel>(this CombatParserContextOne context, List<CombatPlayer> players, Func<CombatPlayer, IEnumerable<TModel>> selector, CancellationToken cancelationToken)
        where TModel : class, ICombatPlayerData
    {
        var combatPlayerData = players.SelectMany(p =>
            selector(p).Select(dd =>
            {
                dd.SetCombatPlayerId(p.Id);
                return dd;
            }
        )).ToList();

        if (combatPlayerData.Count > 0)
        {
            await context.BulkInsertAsync(combatPlayerData, cancellationToken: cancelationToken);
        }
    }

    public static async Task<List<CombatPlayer>> BulkInsertCombatPlayersAsync(this CombatParserContextOne context, int combatId, IEnumerable<CombatPlayer> combatPlayers, CancellationToken cancelationToken)
    {
        var players = combatPlayers.Select(cd =>
        {
            cd.SetCombatId(combatId);

            return cd;
        }).ToList();

        if (players.Count > 0)
        {
            await context.BulkInsertAsync(players, new BulkConfig
            {
                SetOutputIdentity = true
            }, cancellationToken: cancelationToken);
        }

        return players;
    }

    public static async Task BulkInsertCombatAurasAsync(this CombatParserContextOne context, int combatId, IEnumerable<CombatAura> combatAuras, CancellationToken cancelationToken)
    {
        var data = combatAuras.Select(cd =>
        {
            cd.SetCombatId(combatId);

            return cd;
        }).ToList();

        if (data.Count > 0)
        {
            await context.BulkInsertAsync(data, cancellationToken: cancelationToken);
        }
    }

    public static async Task BulkInsertCombatPlayerStatsAsync(this CombatParserContextOne context, List<CombatPlayer> players, CancellationToken cancelationToken)
    {
        var stats = players.Select(p =>
        {
            var stats = p.Stats;
            stats.SetCombatPlayerId(p.Id);

            return stats;
        }).ToList();

        if (stats.Count > 0)
        {
            await context.BulkInsertAsync(stats, cancellationToken: cancelationToken);
        }
    }

    public static async Task BulkInsertCombatPlayerScoresAsync(this CombatParserContextOne context, List<CombatPlayer> players, CancellationToken cancelationToken)
    {
        var scores = players.Select(p =>
        {
            var score = p.Score;
            score?.SetCombatPlayerId(p.Id);

            return score;
        }).Where(s => s != null).ToList();

        if (scores.Count > 0)
        {
            await context.BulkInsertAsync(scores, cancellationToken: cancelationToken);
        }
    }
}
