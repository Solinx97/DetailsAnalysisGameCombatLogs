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
    public override void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var positionOwnerId = combatDataLine[2];
        if (combatDataLine.Length <= 28 || !units.TryGetValue(positionOwnerId, out var unit))
        {
            return;
        }

        var pos1Index = 27;
        var pos2Index = 28;

        if (double.TryParse(combatDataLine[pos1Index], out var positionX)
            && double.TryParse(combatDataLine[pos2Index], out var positionY))
        {
            var position = new UnitPosition
            {
                OwnerGameId = positionOwnerId,
                X = positionX,
                Y = positionY,
                Time = GetTimeFromStart(combatDataLine[0])
            };

            unit.UnitPositions.Add(position);
        }
    }

    public override void GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var absorbeDone = new HealDone
        {
            GameSpellId = int.Parse(combatDataLine[^6]),
            Spell = combatDataLine[^5].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
            Overheal = 0,
            ModificationType = (int)ModificationType.Absorb,
        };

        if (int.TryParse(combatDataLine[^3], out var amountOfHeal))
        {
            absorbeDone.Value = amountOfHeal;
        };

        ApplyUnits(combatDataLine, absorbeDone, units, combatDataLine.Length - 10, 6);

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

        if ((combatDataLine[1].Equals(CombatLogKeyWords.SWING_DAMAGE_LANDED)
            && long.TryParse(combatDataLine[12], out var currentHealth)
            && long.TryParse(combatDataLine[13], out var maxHealth))
            ||
            (long.TryParse(combatDataLine[15], out currentHealth)
            && long.TryParse(combatDataLine[16], out maxHealth)))
        {
            AddUnitHealth(unit, ownerId, currentHealth, maxHealth, combatDataLine[0], status);
        }
    }
}