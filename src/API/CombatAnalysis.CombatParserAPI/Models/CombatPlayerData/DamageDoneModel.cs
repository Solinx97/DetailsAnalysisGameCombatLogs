using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class DamageDoneModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Required]
    public string CreatorGameId { get; set; } = string.Empty;

    [Required]
    public string TargetGameId { get; set; } = string.Empty;

    [Required]
    public string TargetHash { get; set; } = string.Empty;

    public long TargetCurrentHealth { get; set; }

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Mitigated { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
