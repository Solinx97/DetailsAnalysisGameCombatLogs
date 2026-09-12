using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class HealDoneModel : CombatUnitBase
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
    public int Overheal { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public int ModificationType { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
