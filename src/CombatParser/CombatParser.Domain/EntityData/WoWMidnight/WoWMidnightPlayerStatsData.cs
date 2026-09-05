using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.EntityData.WoWMidnight;

public record WoWMidnightPlayerStatsData(
    int Strength,
    int Agility,
    int Intelligence,
    int Stamina,
    int Dodge,
    int Parry,
    int Block,
    int Crit,
    int Haste,
    int Armor,
    int Mastery,
    int Versality,
    int Lifesteal,
    int Avoidance,
    int Movement,
    string Talents,
    int CombatPlayerId
    ) : IPlayerStats;
