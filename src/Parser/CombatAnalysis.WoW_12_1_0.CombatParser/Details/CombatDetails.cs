using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

public class CombatDetails(ICombatParserHelper combatParserHelper, ILogger logger, ConcurrentDictionary<string, Unit> units) 
    : WoW.CombatParser.Details.CombatDetails(combatParserHelper, logger, units)
{
    protected override WoW.CombatParser.Details.CombatDetailsManager CreateCombatDetailsManager(DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var combatDetailsManager = new CombatDetailsManager(_combatParserHelper, combatStarted, combatFinished);
        return combatDetailsManager;
    }
}
