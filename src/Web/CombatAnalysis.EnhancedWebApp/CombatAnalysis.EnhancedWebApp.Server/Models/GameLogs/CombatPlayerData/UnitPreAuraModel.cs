namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class UnitPreAuraModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public int GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int AbilityType { get; set; }

    public int Status { get; set; }

    public string? UnitId { get; set; }
}
