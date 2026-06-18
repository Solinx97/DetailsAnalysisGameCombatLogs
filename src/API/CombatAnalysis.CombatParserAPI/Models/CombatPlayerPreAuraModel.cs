using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerPreAuraModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string CreatorGameId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int AbilityType { get; set; }

    [Range(0, int.MaxValue)]
    public int Status { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
