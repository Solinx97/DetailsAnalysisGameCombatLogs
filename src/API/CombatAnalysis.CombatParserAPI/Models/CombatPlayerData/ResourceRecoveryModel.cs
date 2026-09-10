using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class ResourceRecoveryModel : CombatUnitBase
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameSpellId { get; set; }

    [Required]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
