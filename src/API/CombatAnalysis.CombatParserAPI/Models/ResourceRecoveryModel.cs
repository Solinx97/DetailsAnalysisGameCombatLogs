using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class ResourceRecoveryModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; }

    [Required]
    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; }

    [Required]
    public string Target { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
