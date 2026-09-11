using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities;

public class PlayerStats : IPlayerStats
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

    public int Spirit { get; set; }

    public int Hit { get; set; }

    public int Expertise { get; set; }

    public string Talents { get; set; } = string.Empty;

    public int CombatPlayerId { get; set; }
}
