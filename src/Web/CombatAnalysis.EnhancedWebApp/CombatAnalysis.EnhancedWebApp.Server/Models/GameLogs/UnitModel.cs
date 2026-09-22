namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class UnitModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }
    
    public int CombatId { get; set; }

    public UnitInfoModel UnitInfo { get; set; }

    public List<UnitHealthModel> UnitHealthes { get; set; } = [];

    public List<UnitCastModel> UnitCasts { get; init; } = [];

    public List<UnitPositionModel> UnitPositions { get; set; } = [];
}
