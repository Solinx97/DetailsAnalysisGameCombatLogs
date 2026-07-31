using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class ResourceRecoveryModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; } = string.Empty;

    [Required]
    public string Target { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
