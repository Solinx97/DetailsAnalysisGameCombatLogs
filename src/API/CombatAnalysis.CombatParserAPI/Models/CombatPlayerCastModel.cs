using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerCastModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    [Required]
    public string Creator { get; set; } = string.Empty;

    [Required]
    public string Target { get; set; } = string.Empty;

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
