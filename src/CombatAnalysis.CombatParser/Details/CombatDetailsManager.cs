using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Details;

internal static class CombatDetailsManager
{
    public static CombatPlayerPosition GetPositions(string playerId, List<string> combatDataLine)
    {
        if (!combatDataLine[2].Equals(playerId))
        {
            return null;
        }

        var pos1Index = 25;
        var pos2Index = 24;

        if (combatDataLine[1].Equals(CombatLogKeyWords.SwingDamage)
            || combatDataLine[1].Equals(CombatLogKeyWords.SwingDamageLanded))
        {
            pos1Index = 22;
            pos2Index = 21;
        }

        if (double.TryParse(combatDataLine[pos1Index], out var position1)
            && double.TryParse(combatDataLine[pos2Index], out var position2))
        {
            var position = new CombatPlayerPosition
            {
                PositionX = -position1,
                PositionY = position2
            };

            return position;
        }

        return null;
    }

    public static DamageDone GetPlayerDamageDone(string playerId, List<string> combatDataLine)
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellSummon, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!combatDataLine[2].Contains(CombatLogKeyWords.Player) || !combatDataLine[2].Equals(playerId))
        {
            return null;
        }

        var damageDone = GetDamageDone(playerId, combatDataLine);

        return damageDone;
    }

    public static DamageDone GetPetsDamageDone(string playerId, List<string> combatDataLine, Dictionary<string, List<string>> petsId)
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellSummon, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (combatDataLine[2].Contains(CombatLogKeyWords.Player) ||
            (!combatDataLine[2].Contains(CombatLogKeyWords.Creature) && !combatDataLine[2].Contains(CombatLogKeyWords.Pet)))
        {
            return null;
        }

        var currentPet = string.Empty;
        var petPlayerId = string.Empty;
        foreach (var item in petsId)
        {
            var pets = item.Value;

            currentPet = pets.Where(x => x.Equals(combatDataLine[2])).FirstOrDefault();
            if (!string.IsNullOrEmpty(currentPet))
            {
                petPlayerId = item.Key;
                break;
            }
        }

        if (string.IsNullOrEmpty(petPlayerId) || petPlayerId != playerId)
        {
            return null;
        }

        var spellOrItem = $"{combatDataLine[3].Trim('"')} - ";
        var damageDone = GetDamageDone(playerId, combatDataLine, spellOrItem);

        return damageDone;
    }

    public static HealDone GetHealDone(string playerId, List<string> combatDataLine)
    {
        if (!combatDataLine[2].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatDataLine[^4], out var value3);
        int.TryParse(combatDataLine[^3], out var value4);

        var isCrit = string.Equals(combatDataLine[^1], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var healDone = new HealDone
        {
            Time = TimeSpan.Parse(combatDataLine[0]),
            FromPlayer = combatDataLine[3].Trim('"'),
            ToPlayer = combatDataLine[7].Trim('"'),
            SpellOrItem = combatDataLine[11].Trim('"'),
            DamageAbsorbed = string.Empty,
            ValueWithOverheal = value3,
            Overheal = value4,
            Value = value3 - value4,
            IsFullOverheal = value3 - value4 == 0,
            IsCrit = isCrit
        };

        return healDone;
    }

    public static HealDone GetAbsorb(string playerId, List<string> combatDataLine)
    {
        if (!combatDataLine[10].Equals(playerId) && !combatDataLine[13].Equals(playerId))
        {
            return null;
        }

        var countDataWithMeleeDamage = 19;

        int.TryParse(combatDataLine[^2], out var amountOfHeal);

        var absorbeDone = new HealDone
        {
            Time = TimeSpan.Parse(combatDataLine[0]),
            FromPlayer = combatDataLine[^8].Trim('"'),
            ToPlayer = combatDataLine[7].Trim('"'),
            SpellOrItem = combatDataLine[^4].Trim('"'),
            DamageAbsorbed = combatDataLine.Count > countDataWithMeleeDamage ? combatDataLine[11].Trim('"') : CombatLogKeyWords.MeleeDamage,
            ValueWithOverheal = amountOfHeal,
            Overheal = 0,
            Value = amountOfHeal,
            IsFullOverheal = false,
            IsCrit = false,
            IsAbsorbed = true
        };

        return absorbeDone;
    }

    public static DamageTaken GetDamageTaken(List<string> combatDataLine)
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || !combatDataLine[2].Contains("0000000000000000") || !combatDataLine[2].Contains(CombatLogKeyWords.Creature))
        {
            return null;
        }

        int.TryParse(combatDataLine[^10], out var value);

        var spellOrItem = combatDataLine[1].Equals(CombatLogKeyWords.SwingDamage) || combatDataLine[1].Equals(CombatLogKeyWords.SwingMissed)
            ? CombatLogKeyWords.MeleeDamage : combatDataLine[11].Trim('"');

        var isCrushing = string.Equals(combatDataLine[^1], CombatLogKeyWords.IsCrushing, StringComparison.OrdinalIgnoreCase);

        int realDamage = 0, mitigated = 0, absorb = 0, blocked = 0, resist = 0;
        var index = -1;

        if (string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            index = 13;

            int.TryParse(combatDataLine[^1], out realDamage);
            int.TryParse(combatDataLine[^2], out absorb);
        }
        else if (!string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
        {
            int.TryParse(combatDataLine[^9], out realDamage);
            int.TryParse(combatDataLine[^4], out absorb);
            int.TryParse(combatDataLine[^5], out blocked);
            int.TryParse(combatDataLine[^6], out resist);

            mitigated = realDamage - value;
        }

        var isResist = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
        var isImmune = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        var isAbsorb = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Absorb, StringComparison.OrdinalIgnoreCase);

        var isPeriodicDamage = false;
        var enemy = combatDataLine[3];
        if (string.Equals(combatDataLine[3], "nil", StringComparison.OrdinalIgnoreCase))
        {
            isPeriodicDamage = true;
            enemy = combatDataLine[11];
        }

        var damageTaken = new DamageTaken
        {
            Value = value,
            ActualValue = value + absorb,
            Time = TimeSpan.Parse(combatDataLine[0]),
            FromEnemy = enemy.Trim('"'),
            ToPlayer = combatDataLine[7].Trim('"'),
            SpellOrItem = spellOrItem,
            IsPeriodicDamage = isPeriodicDamage,
            Resisted = resist,
            Absorbed = absorb,
            Blocked = blocked,
            RealDamage = realDamage,
            Mitigated = mitigated < 0 ? 0 : mitigated,
            IsDodge = string.Equals(combatDataLine[^2], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase),
            IsParry = string.Equals(combatDataLine[^2], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase),
            IsMiss = string.Equals(combatDataLine[^2], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase),
            IsResist = isResist,
            IsImmune = isImmune,
            IsAbsorb = isAbsorb,
            IsCrushing = isCrushing,
        };

        return damageTaken;
    }

    public static ResourceRecovery GetEnergyRecovery(string playerId, List<string> combatDataLine)
    {
        if (!combatDataLine[6].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatDataLine[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery);

        var spellOrItem = combatDataLine[1].Contains(CombatLogKeyWords.SpellEnergize) ? combatDataLine[11] : combatDataLine[3];

        var energyRecovery = new ResourceRecovery
        {
            Time = TimeSpan.Parse(combatDataLine[0]),
            Value = amoutOfResourcesRecovery,
            SpellOrItem = spellOrItem.Trim('"')
        };

        return energyRecovery;
    }

    private static DamageDone GetDamageDone(string playerId, List<string> combatDataLine, string spellOrItem = "")
    {
        int.TryParse(combatDataLine[^10], out var amountOfValue);

        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem = CombatLogKeyWords.MeleeDamage;
        }
        else
        {
            spellOrItem = combatDataLine[11].Trim('"');
        }

        var isPeriodicDamage = false;
        if (combatDataLine[1] == CombatLogKeyWords.SpellPeriodicDamage)
        {
            isPeriodicDamage = true;
        }

        var index = -1;
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            index = 13;
        }
        else if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            index = 10;
        }

        var isResist = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
        var isParry = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
        var isDodge = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
        var isImmune = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        var isMiss = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);

        var isCrit = string.Equals(combatDataLine[^3], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var damageDone = new DamageDone
        {
            Value = amountOfValue,
            Time = TimeSpan.Parse(combatDataLine[0]),
            FromPlayer = combatDataLine[3].Trim('"'),
            ToEnemy = combatDataLine[7].Trim('"'),
            SpellOrItem = spellOrItem,
            IsPeriodicDamage = isPeriodicDamage,
            IsDodge = isDodge,
            IsMiss = isMiss,
            IsParry = isParry,
            IsResist = isResist,
            IsImmune = isImmune,
            IsCrit = isCrit,
            IsPet = true,
        };

        return damageDone;
    }
}
