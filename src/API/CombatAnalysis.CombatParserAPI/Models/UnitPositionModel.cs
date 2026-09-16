using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitPositionModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string OwnerGameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    public string? UnitId { get; set; }
}
