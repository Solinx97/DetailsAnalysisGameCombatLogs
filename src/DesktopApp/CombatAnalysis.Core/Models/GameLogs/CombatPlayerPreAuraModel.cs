namespace CombatAnalysis.Core.Models.GameLogs;

public class CombatPlayerPreAuraModel
{
    public int Id { get; set; }

    public string CreatorGameId { get; set; } = string.Empty;

    public int GameId { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
