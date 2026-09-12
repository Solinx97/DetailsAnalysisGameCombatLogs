using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Details;

public interface ICombatDetailsManager
{
    void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, List<CombatPlayerAura>> auras, List<CombatUnit> summonedCreatures);

    void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    HealDone? GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    HealDone? GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    ResourceRecovery? GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    (string, CombatPlayerDeath?) GetPlayerDeath(string[] combatDataLine);

    DamageDone? GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);
}
