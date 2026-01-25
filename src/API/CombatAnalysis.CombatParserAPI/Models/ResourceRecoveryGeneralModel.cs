using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class ResourceRecoveryGeneralModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; }

    [Required]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public double ResourcePerSecond { get; set; }

    [Range(0, int.MaxValue)]
    public int CastNumber { get; set; }

    [Required]
    public int MinValue { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxValue { get; set; }

    [Required]
    public double AverageValue { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
