namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities.CombatPlayerData;

public class SpecializationScore
{
    public double DamageScore { get; set; }

    public int DamageDone { get; set; }

    public double HealScore { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }

    public int CombatPlayerId { get; set; }
}
