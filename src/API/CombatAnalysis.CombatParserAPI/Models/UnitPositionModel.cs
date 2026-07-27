using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitPositionModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
