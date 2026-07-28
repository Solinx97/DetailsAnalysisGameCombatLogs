namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class UnitCastModel
{
    public string Id { get; set; } = string.Empty;

    public string CreatorGameId { get; set; } = string.Empty;

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan Time { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string? TargetGameId { get; set; }

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    public int CombatPlayerId { get; set; }
}
