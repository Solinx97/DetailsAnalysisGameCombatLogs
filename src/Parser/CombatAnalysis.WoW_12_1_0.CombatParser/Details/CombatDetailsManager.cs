using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

internal class CombatDetailsManager(ICombatParserHelper combatParserHelper, string[] playersId, DateTimeOffset combatStarted, DateTimeOffset combatFinished) 
    : WoW.CombatParser.Details.CombatDetailsManager(combatParserHelper, playersId, combatStarted, combatFinished)
{
    private readonly string[] _playersId = playersId;

    public override HealDone? GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[10]))
            && !_playersId.Any(playerId => playerId.Equals(combatDataLine[13])))
        {
            return null;
        }

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