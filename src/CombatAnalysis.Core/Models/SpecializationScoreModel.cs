namespace CombatAnalysis.Core.Models;

public class SpecializationScoreModel
{
    public int SpecId { get; set; }

    public int BossId { get; set; }

    public int Difficult { get; set; }

    public int Damage { get; set; }

    public int Heal { get; set; }

    public DateTimeOffset Updated { get; set; }
}
