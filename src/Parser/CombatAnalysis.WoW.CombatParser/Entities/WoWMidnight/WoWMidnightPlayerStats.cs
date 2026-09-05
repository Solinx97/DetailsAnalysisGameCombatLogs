using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.WoWMidnight;

public class WoWMidnightPlayerStats : IPlayerStats
{
    public int Strength { get; set; }

    public int Agility { get; set; }

    public int Intelligence { get; set; }

    public int Stamina { get; set; }

    public int Dodge { get; set; }

    public int Parry { get; set; }

    public int Block { get; set; }

    public int Crit { get; set; }

    public int Haste { get; set; }

    public int Armor { get; set; }

    public int Mastery { get; set; }

    public int Versality { get; set; }

    public int Lifesteal { get; set; }

    public int Avoidance { get; set; }

    public int Movement { get; set; }

    public string Talents { get; set; } = string.Empty;

    public int CombatPlayerId { get; set; }
}
