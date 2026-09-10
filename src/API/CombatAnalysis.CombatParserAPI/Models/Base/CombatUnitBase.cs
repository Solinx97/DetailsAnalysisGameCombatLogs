namespace CombatAnalysis.CombatParserAPI.Models.Base;

public class CombatUnitBase
{
    public CombatUnitModel? Creator { get; set; }

    public string? CreatorGameId { get; set; }

    public CombatUnitModel? Target { get; set; }

    public string? TargetGameId { get; set; }
}
