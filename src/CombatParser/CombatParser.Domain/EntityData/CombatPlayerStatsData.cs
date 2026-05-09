namespace CombatParser.Domain.EntityData;

public record CombatPlayerStatsData(
    int Strength, 
    int Agility, 
    int Intelligence, 
    int Stamina,
    int Spirit,
    int Dodge,
    int Parry,
    int Crit,
    int Haste, 
    int Hit,
    int Expertise,
    int Armor, 
    string Talents, 
    int CombatPlayerId
    );
