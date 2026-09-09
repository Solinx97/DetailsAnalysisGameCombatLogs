using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatUnitModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string GameId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    [Required]
    public string UnitHash { get; set; } = string.Empty;

    public string? CreatorGameId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
