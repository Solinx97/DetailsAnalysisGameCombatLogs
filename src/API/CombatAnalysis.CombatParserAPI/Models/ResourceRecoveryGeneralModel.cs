using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class ResourceRecoveryGeneralModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public double ResourcePerSecond { get; set; }

    [Range(0, int.MaxValue)]
    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
