using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class DamageTakenGeneralModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public int ActualValue { get; set; }

    [Range(0, int.MaxValue)]
    public double DamageTakenPerSecond { get; set; }

    [Range(0, int.MaxValue)]
    public int CritNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int MissNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int CastNumber { get; set; }

    [Range(0, int.MaxValue)]
    public int MinValue { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxValue { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageValue { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
