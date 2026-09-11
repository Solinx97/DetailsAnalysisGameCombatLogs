using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Interfaces.Details;

public interface ICombatDetailsManager
{
    void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, List<CombatPlayerAura>> auras, List<CombatUnit> summonedCreatures);

    void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, List<UnitCast>> casts);

    void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, List<UnitPosition>> positions);

    (string, HealDone?) GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    (string, HealDone?) GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    (string, ResourceRecovery?) GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    (string, CombatPlayerDeath?) GetPlayerDeath(string[] combatDataLine);

    DamageDone? GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    void GetCombatCreature(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);
}
