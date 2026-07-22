namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class CombatPlayerCastModel
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    public int CombatPlayerId { get; set; }
}
