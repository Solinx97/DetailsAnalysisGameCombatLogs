using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class DamageTakenModel
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

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; } = string.Empty;

    [Required]
    public string Target { get; set; } = string.Empty;

    public bool IsPeriodicDamage { get; set; }

    [Range(0, int.MaxValue)]
    public int Resisted { get; set; }

    [Range(0, int.MaxValue)]
    public int Absorbed { get; set; }

    [Range(0, int.MaxValue)]
    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    [Range(0, int.MaxValue)]
    public int Mitigated { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageTakenType { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
