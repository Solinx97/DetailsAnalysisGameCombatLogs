using CombatAnalysis.WoW.CombatParser.Entities;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces;

public interface ICombatParserHelper
{
    string[] SplitCombatData(string combatData);

    Unit ParseUnits(ConcurrentDictionary<string, Unit> units, string gameId, string name, string unitHash, string? creatorGameId = null);
}
