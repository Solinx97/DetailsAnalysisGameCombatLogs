using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class DamageDoneModel : CombatUnitBase
{
    public string Id { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Overkill { get; set; }

    public int Mitigated { get; set; }

    public UnitModel? Unit { get; set; }

    public string? UnitId { get; set; }
}
