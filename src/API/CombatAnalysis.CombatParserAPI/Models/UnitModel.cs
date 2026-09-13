using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string GameId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }

    [Required]
    public List<UnitHealthModel> UnitHealthes { get; set; } = [];

    [Required]
    public List<UnitCastModel> UnitCasts { get; init; } = [];

    [Required]
    public List<UnitPositionModel> UnitPositions { get; init; } = [];
}
