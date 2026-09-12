namespace CombatAnalysis.WoW.CombatParser.Entities.Base;

public class CombatUnitDataBase
{
    public CombatUnit Creator { get; set; } = new();

    public string CreatorGameId { get; set; }

    public CombatUnit Target { get; set; } = new();

    public string TargetGameId { get; set; }
}
