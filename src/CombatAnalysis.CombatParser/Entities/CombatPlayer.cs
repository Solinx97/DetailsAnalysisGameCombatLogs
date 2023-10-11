namespace CombatAnalysis.CombatParser.Entities;

public class CombatPlayer
{
    public string UserName { get; set; }

    public double AverageItemLevel { get; set; }

    public int EnergyRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int UsedBuffs { get; set; }

    public PlayerStats Stats { get; set; }
}
