namespace CombatAnalysis.UploadingLogsApp.Models;

public class CombatUnitModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    public string UnitHash { get; set; } = string.Empty;

    public string? CreatorGameId { get; set; }

    public int CombatId { get; set; }
}
