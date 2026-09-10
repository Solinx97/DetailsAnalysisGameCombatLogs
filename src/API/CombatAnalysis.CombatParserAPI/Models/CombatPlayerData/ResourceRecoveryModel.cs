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

    public string? CreatorId { get; set; }

    [Required]
    public string CreatorGameId { get; set; }

    public string? TargetId { get; set; }

    [Required]
    public string TargetGameId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
