using CombatAnalysis.CombatParserAPI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CreateCombatModel : CombatBaseModel
{
    public int GameVersion { get; init; }

    [Required]
    public List<CreateCombatPlayerModel> CombatPlayers { get; init; } = [];
}
