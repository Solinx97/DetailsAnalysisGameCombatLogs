using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitHealthModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string OwnerGameId { get; set; } = string.Empty;

    public long CurrentHealth { get; set; }

    [Range(0, int.MaxValue)]
    public long MaxHealth { get; set; }

    public int Status { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public string? UnitId { get; set; }
}
