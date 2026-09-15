using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class UnitPreAuraModel
{
    public string Id { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameId { get; set; }

    [Required]
    public string TargetGameId { get; set; } = string.Empty;

    public int AbilityType { get; set; }

    [Range(0, int.MaxValue)]
    public int Status { get; set; }

    public string? UnitId { get; set; }
}
