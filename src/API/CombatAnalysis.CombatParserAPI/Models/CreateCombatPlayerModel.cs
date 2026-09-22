using CombatAnalysis.CombatParserAPI.Models.Base;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CreateCombatPlayerModel : CombatPlayerBaseModel
{
    public string UnitGameId { get; set; } = string.Empty;
}
