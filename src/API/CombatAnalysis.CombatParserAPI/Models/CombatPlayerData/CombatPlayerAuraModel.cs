using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class CombatPlayerAuraModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameAuraId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Creator { get; set; } = string.Empty;

    [Required]
    public string Target { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int AuraCreatorType { get; set; }

    [Range(0, int.MaxValue)]
    public int AuraType { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    [Range(0, int.MaxValue)]
    public int Stacks { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
