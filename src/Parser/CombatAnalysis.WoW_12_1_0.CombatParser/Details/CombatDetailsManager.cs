using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

internal class CombatDetailsManager(ICombatParserHelper combatParserHelper, DateTimeOffset combatStarted, DateTimeOffset combatFinished) 
    : WoW.CombatParser.Details.CombatDetailsManager(combatParserHelper, combatStarted, combatFinished)
{
    public override void GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var absorbeDone = new HealDone
        {
            GameSpellId = int.Parse(combatDataLine[^6]),
            Spell = combatDataLine[^4].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
            Overheal = 0,
            ModificationType = (int)ModificationType.Absorb,
        };

        if (int.TryParse(combatDataLine[^2], out var amountOfHeal))
        {
            absorbeDone.Value = amountOfHeal;
        };

        ApplyUnits(combatDataLine, absorbeDone, units, 10, 2);

        if (units.TryGetValue(absorbeDone.CreatorGameId, out var creatorUnit))
        {
            creatorUnit.HealDones.Add(absorbeDone);
        }
    }

    public override void GetHealth(string[] combatDataLine, ConcurrentDictionary<string, Unit> units, UnitHealthStatus status)
    {
        var ownerId = combatDataLine[6];
        if (!units.TryGetValue(ownerId, out var unit))
        {
            return;
        }

        if (combatDataLine[1].Equals(CombatLogKeyWords.SwingDamageLanded)
            && long.TryParse(combatDataLine[12], out var currentHealth)
            && long.TryParse(combatDataLine[13], out var maxHealth))
        {
            AddUnitHealth(unit, ownerId, currentHealth, maxHealth, combatDataLine[0], status);
        }
        else if (long.TryParse(combatDataLine[15], out currentHealth)
            && long.TryParse(combatDataLine[16], out maxHealth))
        {
            AddUnitHealth(unit, ownerId, currentHealth, maxHealth, combatDataLine[0], status);
        }
    }
}