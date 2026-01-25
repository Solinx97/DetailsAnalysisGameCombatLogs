using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerDeathModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    public string LastHitSpell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int LastHitValue { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
