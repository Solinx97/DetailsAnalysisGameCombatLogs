namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string PlayerId { get; set; }

    public double AverageItemLevel { get; set; }

    public int EnergyRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int UsedBuffs { get; set; }

    public int CombatId { get; set; }
}
