using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Entities.WoWMidnight;
using CombatParser.Domain.Entities.WoWMoPClassic;
using CombatParser.Domain.EntityData;
using CombatParser.Domain.EntityData.WoWMidnight;
using CombatParser.Domain.EntityData.WoWMoPClassic;
using CombatParser.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Entities;

public class CombatPlayer : CombatDataBase
{
    private CombatPlayer() { }

    private CombatPlayer(double averageItemLevel, string playerId, string unitGameId)
    {
        AverageItemLevel = averageItemLevel;
        PlayerId = playerId;
        UnitGameId = unitGameId;
    }

    public int Id { get; private set; }

    public double AverageItemLevel { get; private set; }

    public SpecializationScore? Score { get; private set; }

    public Player Player { get; private set; }

    public string PlayerId { get; private set; } = string.Empty;

    public Unit Unit { get; private set; }

    public string UnitId { get; private set; } = string.Empty;

    public Combat Combat { get; private set; }

    [NotMapped]
    public IPlayerStats Stats { get; private set; }

    [NotMapped]
    public string UnitGameId { get; private set; } = string.Empty;

    public void SetUnitId(string unitId)
    {
        UnitId = unitId;
    }

    public static CombatPlayer Create(double averageItemLevel, string playerId, IPlayerStatsData stats, SpecializationScoreData score, string unitGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(playerId, nameof(playerId));
        ArgumentOutOfRangeException.ThrowIfNegative(averageItemLevel, nameof(averageItemLevel));
        ArgumentException.ThrowIfNullOrEmpty(unitGameId, nameof(unitGameId));

        var combatPlayer = new CombatPlayer(averageItemLevel, playerId, unitGameId);

        AddCombatPlayerData(combatPlayer, stats, score);

        return combatPlayer;
    }

    private static void AddCombatPlayerData(CombatPlayer combatPlayer, IPlayerStatsData stats, SpecializationScoreData score)
    {
        combatPlayer.AddStats(stats);
        combatPlayer.AddSpecializationScore(score);
    }

    private void AddStats(IPlayerStatsData stats)
    {
        IPlayerStats createdStats;
        switch (stats)
        {
            case WoWMoPClassicPlayerStatsData mop:
                createdStats = WoWMoPClassicPlayerStats.Create(mop.Strength, mop.Agility, mop.Intelligence, mop.Stamina, mop.Spirit,
                    mop.Dodge, mop.Parry, mop.Block, mop.Crit, mop.Haste, mop.Hit,
                    mop.Expertise, mop.Armor, mop.Talents);
                break;
            case WoWMidnightPlayerStatsData midnight:
                createdStats = WoWMidnightPlayerStats.Create(midnight.Strength, midnight.Agility, midnight.Intelligence, midnight.Stamina,
                    midnight.Dodge, midnight.Parry, midnight.Block, midnight.Crit, midnight.Haste, midnight.Mastery, midnight.Versality,
                    midnight.Lifesteal, midnight.Avoidance, midnight.Movement, midnight.Armor, midnight.Talents);
                break;
            default:
                throw new InvalidOperationException($"Unknown stats type: {stats?.GetType().Name}");
        }

        Stats = createdStats;
    }

    private void AddSpecializationScore(SpecializationScoreData score)
    {
        if (score == null)
        {
            return;
        }

        var createdScore = SpecializationScore.Create(score.DamageScore, score.DamageDone, score.HealScore, score.HealDone, score.Updated,
            score.SpecializationId);
        Score = createdScore;
    }
}
