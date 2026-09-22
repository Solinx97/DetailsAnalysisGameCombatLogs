using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class HealDoneGeneralModel
{
    public string Id { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public double HealPerSecond { get; set; }

    [Range(0, int.MaxValue)]
    public int CritNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int CastNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int MinValue { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxValue { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageValue { get; set; }

    public string? UnitId { get; set; }
}
