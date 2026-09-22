namespace CombatAnalysis.CombatParserAPI.Models;

public class SpecializationScoreModel
{
    public int Id { get; set; }

    public double DamageScore { get; set; }

    public long DamageDone { get; set; }

    public double HealScore { get; set; }

    public long HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }

    public int CombatPlayerId { get; set; }
}
