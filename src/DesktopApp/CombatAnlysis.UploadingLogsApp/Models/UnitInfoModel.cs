namespace CombatAnalysis.UploadingLogsApp.Models;

public class UnitInfoModel
{
    public string Id { get; set; } = string.Empty;

    public long ResourcesRecovery { get; set; }

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public string UnitId { get; set; } = string.Empty;
}
