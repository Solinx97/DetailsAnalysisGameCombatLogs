namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatPlayerPreAuraModel
{
    public int Id { get; set; }

    public string CreatorGameId { get; set; } = string.Empty;

    public int GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int AbilityType { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
