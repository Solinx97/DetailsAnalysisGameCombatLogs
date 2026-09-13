using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

internal class CombatDetailsManager(ICombatParserHelper combatParserHelper, DateTimeOffset combatStarted, DateTimeOffset combatFinished) 
    : WoW.CombatParser.Details.CombatDetailsManager(combatParserHelper, combatStarted, combatFinished)
{
    public override HealDone GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
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

        ApplyUnits(combatDataLine, absorbeDone, units);

        return absorbeDone;
    }
}