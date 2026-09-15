namespace CombatAnalysis.UploadingLogsApp.Models.CombatPlayerData;

public class UnitPreAuraModel
{
    public string Id { get; set; } = string.Empty;

    public string TargetGameId { get; set; } = string.Empty;

    public int GameId { get; set; }

    public int Status { get; set; }

    public string UnitId { get; set; } = string.Empty;
}
