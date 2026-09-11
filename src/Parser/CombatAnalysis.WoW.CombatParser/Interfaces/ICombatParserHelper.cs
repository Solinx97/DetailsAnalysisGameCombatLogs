using CombatAnalysis.WoW.CombatParser.Entities;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces;

public interface ICombatParserHelper
{
    string[] SplitCombatData(string combatData);

    CombatUnit ParseUnits(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units, bool isSummon = true);
}
