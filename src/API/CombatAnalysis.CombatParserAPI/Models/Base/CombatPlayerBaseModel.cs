using CombatAnalysis.CombatParserAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.Base;

public class CombatPlayerBaseModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageItemLevel { get; set; }

    [Required]
    public IPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    [Required]
    public PlayerModel Player { get; set; } = new();

    public UnitModel? Unit { get; set; }

    public string? UnitId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }
}
