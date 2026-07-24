using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class DamageDoneGeneralModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public double DamagePerSecond { get; set; }

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

    public bool IsPet { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
