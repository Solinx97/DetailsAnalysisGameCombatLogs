using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Extensions;

internal static class CombatParserContextOneExtension
{
    public static async Task BulkInsertUnitTargetDataAsync<TModel>(this CombatParserContextOne context, List<Unit> units, Dictionary<string, string> unitsByGameId, Func<Unit, IEnumerable<TModel>> selector, CancellationToken cancelationToken)
        where TModel : class, ICombatUnitRefs, IUnitTargetRefs
    {
        var combatPlayerData = units.SelectMany(u  =>
            selector(u).Select(result =>
            {
                if (!unitsByGameId.TryGetValue(result.TargetGameId, out var targetId))
                {
                    return null;
                }

                result.SetTargetUnitId(targetId);
                result.SetUnitId(u.Id);

                return result;
            }
        ))
            .Where(x => x != null)
            .ToList();

        if (combatPlayerData.Count > 0)
        {
            await context.BulkInsertAsync(combatPlayerData, cancellationToken: cancelationToken);
        }
    }

    public static async Task<List<Unit>> BulkInsertUnitsAsync(this CombatParserContextOne context, Combat combat, Func<Combat, IEnumerable<Unit>> selector, CancellationToken cancelationToken)
    {
        var combatData = selector(combat).Select(cr =>
        {
            cr.SetCombatId(combat.Id);
            return cr;
        }).ToList();

        if (combatData.Count > 0)
        {
            await context.BulkInsertAsync(combatData, new BulkConfig
            {
                SetOutputIdentity = true
            }, cancellationToken: cancelationToken);
        }

        return combatData;
    }

    public static async Task BulkInsertCombatDataAsync<TModel>(this CombatParserContextOne context, IEnumerable<Unit> combatUnits, Func<Unit, IEnumerable<TModel>> selector, CancellationToken cancelationToken)
        where TModel : class, ICombatUnitRefs
    {
        var combatUnitData = combatUnits.SelectMany(p =>
            selector(p).Select(u =>
            {
                u.SetUnitId(p.Id);

                return u;
            }
        )).ToList();

        if (combatUnitData.Count > 0)
        {
            await context.BulkInsertAsync(combatUnitData, cancellationToken: cancelationToken);
        }
    }

    public static async Task BulkInsertUnitInfoAsync(this CombatParserContextOne context, IEnumerable<Unit> combatUnits, CancellationToken cancelationToken)
    {
        var combatUnitData = combatUnits.Select(p =>
        {
            p.UnitInfo.SetUnitId(p.Id);

            return p.UnitInfo;
        }).ToList();

        if (combatUnitData.Count > 0)
        {
            await context.BulkInsertAsync(combatUnitData, cancellationToken: cancelationToken);
        }
    }

    public static async Task<List<CombatPlayer>> BulkInsertCombatPlayersAsync(this CombatParserContextOne context, int combatId, Dictionary<string, string> unitsByGameId, IEnumerable<CombatPlayer> combatPlayers, CancellationToken cancelationToken)
    {
        var players = combatPlayers.Select(cp =>
        {
            if (!unitsByGameId.TryGetValue(cp.UnitGameId, out var unitId))
            {
                return null;
            }

            cp.SetUnitId(unitId);
            cp.SetCombatId(combatId);

            return cp;
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

    public static async Task BulkInsertCombatPlayerStatsAsync(this CombatParserContextOne context, IEnumerable<CombatPlayer> players, CancellationToken cancelationToken)
    {
        var stats = players.Select<CombatPlayer, IPlayerStats>(p =>
        {
            switch (p.Stats)
            {
                case WoWMoPClassicPlayerStats mop:
                    {
                        var stats = (WoWMoPClassicPlayerStats)p.Stats;
                        stats.SetCombatPlayerId(p.Id);
                        return stats;
                    }

                case WoWMidnightPlayerStats midnight:
                    {
                        var stats = (WoWMidnightPlayerStats)p.Stats;
                        stats.SetCombatPlayerId(p.Id);
                        return stats;
                    }

                default:
                    throw new InvalidOperationException(
                        $"Unknown stats type: {p.Stats?.GetType().Name}");
            }
        }).ToList();

        if (stats.Count > 0)
        {
            await context.BulkInsertAsync(stats, cancellationToken: cancelationToken);
        }
    }

    public static async Task BulkInsertCombatPlayerScoresAsync(this CombatParserContextOne context, int bossId, IEnumerable<CombatPlayer> players, CancellationToken cancelationToken)
    {
        var scores = players.Select(p =>
        {
            var score = p.Score;
            score?.SetCombatPlayerId(p.Id);

            return score;
        }).Where(s => s != null).ToList();

        var bestScores = await context.Set<BestSpecializationScore>()
            .Where(x => x.BossId == bossId).ToListAsync(cancellationToken: cancelationToken);

        foreach (var score in scores)
        {
            var selectedBestScore = bestScores.FirstOrDefault(bs => bs.SpecializationId == score!.SpecializationId);

            var bestSpecialziationDamageDone = selectedBestScore == null ? 0 : selectedBestScore.DamageDone;
            var bestSpecialziationHealDone = selectedBestScore == null ? 0 : selectedBestScore.HealDone;
            score!.SetScore(bestSpecialziationDamageDone, bestSpecialziationHealDone);
        }

        if (scores.Count > 0)
        {
            await context.BulkInsertAsync(scores, cancellationToken: cancelationToken);
        }
    }

    public static async Task BulkUpdateBestSpecializationScoreAsync(this CombatParserContextOne context, int bossId, IEnumerable<CombatPlayer> players, CancellationToken cancelationToken)
    {
        var bestScores = await context.Set<BestSpecializationScore>()
            .Where(x => x.BossId == bossId).ToListAsync(cancellationToken: cancelationToken);
        var scores = players.Select(p => p.Score)
            .Where(s => s != null).ToList();

        foreach (var score in scores)
        {
            var selectedBestScore = bestScores.FirstOrDefault(bs => bs.SpecializationId == score!.SpecializationId);
            if (selectedBestScore != null 
                && (selectedBestScore.DamageDone < score!.DamageDone || selectedBestScore.HealDone < score.HealDone))
            {
                selectedBestScore.Update(score.DamageDone, score.HealDone);
            }
        }

        if (bestScores.Count > 0)
        {
            await context.BulkUpdateAsync(bestScores, cancellationToken: cancelationToken);
        }
    }
}
