using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Details;

internal class CombatDetailsManager(string[] playersId, DateTimeOffset combatStarted, DateTimeOffset combatFinished) : WoW.CombatParser.Details.CombatDetailsManager(playersId, combatStarted, combatFinished)
{
    private readonly string[] _playersId = playersId;

    public override (string, HealDone?) GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[10])) 
            && !_playersId.Any(playerId => playerId.Equals(combatDataLine[13])))
        {
            return (string.Empty, null);
        }

        var absorbeDone = new HealDone
        {
            GameSpellId = int.Parse(combatDataLine[^5]),
            Spell = combatDataLine[^4].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
            Overheal = 0,
            IsCrit = false,
            IsAbsorbed = true
        };

        ApplyUnits(combatDataLine, absorbeDone, units);
        if (int.TryParse(combatDataLine[^2], out var amountOfHeal))
        {
            absorbeDone.Value = amountOfHeal;
        }

        var playerId = _playersId.Any(playerId => playerId.Equals(combatDataLine[10])) ? combatDataLine[10] : combatDataLine[13];

        return (playerId, absorbeDone);
    }
}