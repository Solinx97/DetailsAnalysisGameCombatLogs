using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitCastModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string CreatorGameId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    public string? TargetGameId { get; set; }

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
