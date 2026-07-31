using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class PlayerModel
{
    public string Id { get; set; }

    [Required]
    public string GameId { get; set; }

    [Required]
    public string Username { get; set; }

    [Range(0, int.MaxValue)]
    public int Faction { get; set; }
}
