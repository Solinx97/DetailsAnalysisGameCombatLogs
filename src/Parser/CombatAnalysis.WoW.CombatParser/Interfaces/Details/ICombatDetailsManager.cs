using CombatAnalysis.WoW.CombatParser.Entities;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Details;

public interface ICombatDetailsManager
{
    void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetHealth(string[] combatDataLine, ConcurrentDictionary<string, Unit> units, bool isDamage = true);

    void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void AddUnitDeath(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    void GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);
}
