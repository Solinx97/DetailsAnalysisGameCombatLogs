using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class ResourceRecoveryModel : CombatUnitBase
{
    public string Id { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public int ModificationType { get; set; }

    public string? UnitId { get; set; }
}
