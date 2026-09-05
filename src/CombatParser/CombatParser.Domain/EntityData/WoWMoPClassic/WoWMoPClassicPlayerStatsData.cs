using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.EntityData.WoWMoPClassic;

public record WoWMoPClassicPlayerStatsData(
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
    int Spirit,
    int Hit,
    int Expertise,
    string Talents,
    int CombatPlayerId
    ) : IPlayerStats;
