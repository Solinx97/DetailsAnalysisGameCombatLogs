using CombatAnalysis.WoW.CombatParser.Entities;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

public interface ICombatUnitRefs
{
    string CreatorGameId { get; set; }

    Unit Creator { get; set; }

    string TargetGameId { get; set; }

    Unit Target { get; set; }
}
