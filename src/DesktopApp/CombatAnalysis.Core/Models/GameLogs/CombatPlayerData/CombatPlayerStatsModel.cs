namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class CombatPlayerStatsModel
{
    public int Id { get; set; }

    public int Strength { get; set; }

    public int Agility { get; set; }

    public int Intelligence { get; set; }

    public int Stamina { get; set; }

    public int Spirit { get; set; }

    public int Dodge { get; set; }

    public int Parry { get; set; }

    public int Crit { get; set; }

    public int Haste { get; set; }

    public int Hit { get; set; }

    public int Expertise { get; set; }

    public int Armor { get; set; }

    public string Talents { get; set; }

    public int CombatPlayerId { get; set; }
}
