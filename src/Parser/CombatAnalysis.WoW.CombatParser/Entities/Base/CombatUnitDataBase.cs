using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.Base;

public class CombatUnitDataBase : ICombatUnitRefs
{
    public CombatUnit Creator { get; set; } = new();

    public string CreatorGameId { get; set; }

    public CombatUnit Target { get; set; } = new();

    public string TargetGameId { get; set; }
}
