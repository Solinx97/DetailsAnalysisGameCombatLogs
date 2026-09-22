using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class ResourceRecoveryGeneralModel
{
    public string Id { get; set; } = string.Empty;

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

    public string? UnitId { get; set; }
}
