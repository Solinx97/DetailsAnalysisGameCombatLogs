namespace CombatAnalysis.CombatParserAPI.Models.Base;

public class CombatUnitBase
{
    public UnitModel? Creator { get; set; }

    public string? CreatorGameId { get; set; }

    public UnitModel? Target { get; set; }

    public string? TargetGameId { get; set; }
}
