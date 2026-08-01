using System;

namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class SpecializationScoreModel
{
    public int Id { get; set; }

    public double DamageScore { get; set; }

    public int DamageDone { get; set; }

    public double HealScore { get; set; }

    public int HealDone { get; set; }

    public DateTimeOffset? Updated { get; set; }

    public int SpecializationId { get; set; }

    public int CombatPlayerId { get; set; }
}
