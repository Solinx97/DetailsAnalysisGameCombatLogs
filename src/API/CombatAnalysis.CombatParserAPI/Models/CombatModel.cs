using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel : CombatBaseModel
{
    [Required]
    public List<CombatPlayerModel> CombatPlayers { get; init; } = [];
}
