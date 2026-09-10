using CombatAnalysis.WoW.CombatParser.Entities;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

public interface ICombatUnitRefs
{
    string CreatorGameId { get; set; }

    CombatUnit Creator { get; set; }

    string TargetGameId { get; set; }

    CombatUnit Target { get; set; }
}
