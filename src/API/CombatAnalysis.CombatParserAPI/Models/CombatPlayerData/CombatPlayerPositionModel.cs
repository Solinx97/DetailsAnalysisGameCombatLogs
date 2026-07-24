using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class CombatPlayerPositionModel
{
    public string Id { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    [Required]
    public TimeSpan Time { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
