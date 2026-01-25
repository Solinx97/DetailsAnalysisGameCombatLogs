using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class DamageTakenModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; }

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Range(0, int.MaxValue)]
    public int ActualValue { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string Creator { get; set; }

    [Required]
    public string Target { get; set; }

    [Required]
    public bool IsPeriodicDamage { get; set; }

    [Range(0, int.MaxValue)]
    public int Resisted { get; set; }

    [Range(0, int.MaxValue)]
    public int Absorbed { get; set; }

    [Range(0, int.MaxValue)]
    public int Blocked { get; set; }

    [Required]
    public int RealDamage { get; set; }

    [Range(0, int.MaxValue)]
    public int Mitigated { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageTakenType { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
