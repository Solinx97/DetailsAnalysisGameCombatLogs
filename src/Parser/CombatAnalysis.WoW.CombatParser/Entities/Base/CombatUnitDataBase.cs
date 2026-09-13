namespace CombatAnalysis.WoW.CombatParser.Entities.Base;

public class CombatUnitDataBase
{
    public Unit Creator { get; set; } = new();

    public string CreatorGameId { get; set; }

    public Unit Target { get; set; } = new();

    public string TargetGameId { get; set; }
}
