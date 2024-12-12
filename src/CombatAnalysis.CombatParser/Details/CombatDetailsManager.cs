using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Enums;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Details;

internal class CombatDetailsManager
{
    private readonly List<string> _playersId;
    private readonly DateTimeOffset _combatStarted;
    private readonly DateTimeOffset _combatFinished;

    public CombatDetailsManager(List<string> playersId, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        _playersId = playersId;
        _combatStarted = combatStarted;
        _combatFinished = combatFinished;
    }

    public (string, CombatAura) GetAuras(List<string> combatDataLine, Dictionary<string, List<CombatAura>> auras, List<string> petsId)
    {
        if (combatDataLine[1].Equals(CombatLogKeyWords.AuraRemoved)
            && auras.TryGetValue(combatDataLine[2], out var playerBuffs))
        {
            RemoveAura(combatDataLine, playerBuffs);

            return (string.Empty, null);
        }

        var startTime = GetTimeFromStart(combatDataLine[0]);
        var finishTime = GetTimeFromStart(_combatFinished.ToString());
        var auraType = SelectAuraType(combatDataLine);
        var auraCreatorType = SelectAuraCreatorType(combatDataLine[2], petsId);

        var buff = new CombatAura
        {
            Name = combatDataLine[11],
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            StartTime = startTime,
            FinishTime = finishTime,
            AuraCreatorType = (int)auraCreatorType,
            AuraType = (int)auraType
        };
 
        if (combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose) && int.TryParse(combatDataLine[14], out var stacks))
        {
            buff.Stacks = stacks;
        }

        return (combatDataLine[2], buff);
    }

    public (string, CombatPlayerPosition) GetPositions(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[2]))
            || combatDataLine.Count <= 25)
        {
            return (string.Empty, null);
        }

        var pos1Index = 24;
        var pos2Index = 25;

        if (combatDataLine[1].Equals(CombatLogKeyWords.SwingDamage)
            || combatDataLine[1].Equals(CombatLogKeyWords.SwingDamageLanded))
        {
            pos1Index = 21;
            pos2Index = 22;
        }

        if (double.TryParse(combatDataLine[pos1Index], out var position1)
            && double.TryParse(combatDataLine[pos2Index], out var position2))
        {
            var position = new CombatPlayerPosition
            {
                PositionX = -position2,
                PositionY = position1,
                Time = GetTimeFromStart(combatDataLine[0])
            };

            return (combatDataLine[2], position);
        }

        return (string.Empty, null);
    }

    public (string, DamageDone) GetPlayerDamageDone(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[2]))
            || combatDataLine[6].Contains("0000000000000000"))
        {
            return (string.Empty, null);
        }

        var damageDone = GetDamageDone(combatDataLine, false);

        return (combatDataLine[2], damageDone);
    }

    public (string, DamageDone) GetPetsDamageDone(List<string> combatDataLine, Dictionary<string, List<string>> petsId)
    {
        if (combatDataLine[2].Contains(CombatLogKeyWords.Player) ||
            (!combatDataLine[2].Contains(CombatLogKeyWords.Creature) && !combatDataLine[2].Contains(CombatLogKeyWords.Pet)))
        {
            return (string.Empty, null);
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

        if (string.IsNullOrEmpty(petPlayerId) || !_playersId.Any(playerId => playerId.Equals(petPlayerId)))
        {
            return (string.Empty, null);
        }

        var spellOrItem = $"{combatDataLine[3].Trim('"')} - ";
        var damageDone = GetDamageDone(combatDataLine, true, spellOrItem);

        return (petPlayerId, damageDone);
    }

    public (string, HealDone) GetHealDone(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[2])))
        {
            return (string.Empty, null);
        }

        int.TryParse(combatDataLine[^4], out var value3);
        int.TryParse(combatDataLine[^3], out var value4);

        var isCrit = combatDataLine[^1].Contains(CombatLogKeyWords.IsCrit);

        var healDone = new HealDone
        {
            Spell = combatDataLine[11].Trim('"'),
            Value = value3,
            Overheal = value4,
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            IsCrit = isCrit
        };

        return (combatDataLine[2], healDone);
    }

    public (string, HealDone) GetAbsorb(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[10])) 
            && !_playersId.Any(playerId => playerId.Equals(combatDataLine[13])))
        {
            return (string.Empty, null);
        }

        var countDataWithMeleeDamage = 19;

        var absorbeDone = new HealDone
        {
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[^8].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            Spell = combatDataLine[^4].Trim('"'),
            Overheal = 0,
            IsCrit = false,
            IsAbsorbed = true
        };

        if (int.TryParse(combatDataLine[^2], out var amountOfHeal))
        {
            absorbeDone.Value = amountOfHeal;
        }

        var playerId = _playersId.Any(playerId => playerId.Equals(combatDataLine[10])) ? combatDataLine[10] : combatDataLine[13];

        return (playerId, absorbeDone);
    }

    public (string, DamageTaken) GetDamageTaken(List<string> combatDataLine)
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase))
        {
            return (string.Empty, null);
        }

        if (!combatDataLine[2].Contains("0000000000000000") && !combatDataLine[2].Contains(CombatLogKeyWords.Creature))
        {
            return (string.Empty, null);
        }

        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[6])))
        {
            return (string.Empty, null);
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

        var isDodge = string.Equals(combatDataLine[^2], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
        var isParry = string.Equals(combatDataLine[^2], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
        var isMiss = string.Equals(combatDataLine[^2], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        var isResist = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
        var isImmune = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        var isAbsorb = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Absorb, StringComparison.OrdinalIgnoreCase);

        var damageTakenType = isCrushing ? DamageTakenType.Crushing : DamageTakenType.Normal;
        damageTakenType = isDodge ? DamageTakenType.Dodge : damageTakenType;
        damageTakenType = isParry ? DamageTakenType.Parry : damageTakenType;
        damageTakenType = isMiss ? DamageTakenType.Miss : damageTakenType;
        damageTakenType = index >= 0 && isResist ? DamageTakenType.Resist : damageTakenType;
        damageTakenType = index >= 0 && isImmune ? DamageTakenType.Immune : damageTakenType;
        damageTakenType = index >= 0 && isMiss ? DamageTakenType.Miss : damageTakenType;

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
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = enemy.Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            Spell = spellOrItem,
            IsPeriodicDamage = isPeriodicDamage,
            Resisted = resist,
            Absorbed = absorb,
            Blocked = blocked,
            RealDamage = realDamage,
            Mitigated = mitigated < 0 ? 0 : mitigated,
            DamageTakenType = (int)damageTakenType,
        };

        return (combatDataLine[6], damageTaken);
    }

    public (string, ResourceRecovery) GetEnergyRecovery(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[6])))
        {
            return (string.Empty, null);
        }

        var spellOrItem = combatDataLine[1].Contains(CombatLogKeyWords.SpellEnergize) ? combatDataLine[11] : combatDataLine[3];

        var energyRecovery = new ResourceRecovery
        {
            Time = GetTimeFromStart(combatDataLine[0]),
            Spell = spellOrItem.Trim('"')
        };

        if (int.TryParse(combatDataLine[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery))
        {
            energyRecovery.Value = amoutOfResourcesRecovery;
        }

        return (combatDataLine[6], energyRecovery);
    }

    public (string, PlayerDeath) GetPlayerDeath(List<string> combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[6])))
        {
            return (string.Empty, null);
        }

        var userDeath = new PlayerDeath
        {
            Username = combatDataLine[7].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
        };

        return (combatDataLine[6], userDeath);
    }

    private DamageDone GetDamageDone(List<string> combatDataLine, bool isPet, string spellOrItem = "")
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem += CombatLogKeyWords.MeleeDamage;
        }
        else
        {
            spellOrItem += combatDataLine[11].Trim('"');
        }

        var isPeriodicDamage = false;
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellPeriodicDamage, StringComparison.OrdinalIgnoreCase))
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

        var isCrit = string.Equals(combatDataLine[^3], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var isResist = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
        var isParry = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
        var isDodge = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
        var isImmune = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        var isMiss = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);

        var damageType = isCrit ? DamageType.Crit : DamageType.Normal;
        damageType = isResist ? DamageType.Resist : damageType;
        damageType = isParry ? DamageType.Parry : damageType;
        damageType = isDodge ? DamageType.Dodge : damageType;
        damageType = isImmune ? DamageType.Immune : damageType;
        damageType = isMiss ? DamageType.Miss : damageType;

        var damageDone = new DamageDone
        {
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            Spell = spellOrItem,
            IsPeriodicDamage = isPeriodicDamage,
            DamageType = (int)damageType,
            IsPet = isPet,
        };

        if (int.TryParse(combatDataLine[^10], out var amountOfValue))
        {
            damageDone.Value = amountOfValue;
        }

        return damageDone;
    }

    private void RemoveAura(List<string> combatDataLine, List<CombatAura> auras)
    {
        var playerBuffFound = auras.FirstOrDefault(x => x.Name.Equals(combatDataLine[11]));
        if (playerBuffFound != null)
        {
            playerBuffFound.FinishTime = GetTimeFromStart(combatDataLine[0]);
        }
    }

    private static AuraType SelectAuraType(List<string> combatDataLine)
    {
        if (combatDataLine[2].Equals(combatDataLine[6]))
        {
            if (combatDataLine[13].Contains(CombatLogKeyWords.Debuff))
            {
                return AuraType.MyselfDebuff;
            }

            return AuraType.MyselfBuff;
        }
        else if (combatDataLine[6].StartsWith(CombatLogKeyWords.Pet))
        {
            if (combatDataLine[13].Contains(CombatLogKeyWords.Debuff))
            {
                return AuraType.PetDebuff;
            }

            return AuraType.PetBuff;
        }
        else if (combatDataLine[2].StartsWith(CombatLogKeyWords.Player) 
            && combatDataLine[6].StartsWith(CombatLogKeyWords.Creature))
        {
            if (combatDataLine[13].Contains(CombatLogKeyWords.Debuff))
            {
                return AuraType.EnemyDebuff;
            }

            return AuraType.AllyCreatureBuff;
        }
        else
        {
            if (combatDataLine[13].Contains(CombatLogKeyWords.Debuff))
            {
                return AuraType.AllyDebuff;
            }

            return AuraType.AllyBuff;
        }
    }

    private static AuraCreatorType SelectAuraCreatorType(string creatorId, List<string> petsId)
    {
        if (creatorId.Contains(CombatLogKeyWords.Player))
        {
            return AuraCreatorType.Player;
        }
        else if (creatorId.Contains(CombatLogKeyWords.Pet))
        {
            return AuraCreatorType.Pet;
        }
        else if (petsId.Contains(creatorId))
        {
            return AuraCreatorType.AllyCreature;
        }
        else
        {
            return AuraCreatorType.EnemyCreature;
        }
    }

    private TimeSpan GetTimeFromStart(string time)
    {
        if (DateTimeOffset.TryParse(time, CultureInfo.GetCultureInfo("en-EN"), DateTimeStyles.AssumeUniversal, out var startTime))
        {
            var timeFromStart = startTime - _combatStarted;

            return timeFromStart;
        }

        return TimeSpan.Zero;
    }
}