using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitHealthModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string CreatorGameId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int CurrentHealth { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxHealth { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public bool IsDead { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
