namespace CombatParser.Application.DTOs;

public class CombatPlayerCastDto
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan? StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public int CombatPlayerId { get; set; }
}
