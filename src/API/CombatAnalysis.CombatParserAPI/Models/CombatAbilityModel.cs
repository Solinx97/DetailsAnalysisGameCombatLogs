using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatAbilityModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameId { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0, int.MaxValue)]
    public int AbilityType { get; set; }
}
