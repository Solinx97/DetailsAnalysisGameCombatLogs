namespace CombatAnalysis.Core.Models;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int EnergyRecovery { get; set; }

    public double DamageDonePerSecond { get; set; }

    public double HealDonePerSecond { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public double EnergyRecoveryPerSecond { get; set; }

    public int UsedBuffs { get; set; }

    public int CombatId { get; set; }
}
