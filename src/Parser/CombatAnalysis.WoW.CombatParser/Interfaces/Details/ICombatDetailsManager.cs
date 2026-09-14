using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Details;

public interface ICombatDetailsManager
{
    void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, List<CombatPlayerAura>> auras, List<Unit> summonedCreatures);

    void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetHealth(string[] combatDataLine, ConcurrentDictionary<string, Unit> units, bool isDamage = true);

    void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    HealDone GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    HealDone GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    ResourceRecovery GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void AddUnitDeath(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    DamageDone GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);
}
