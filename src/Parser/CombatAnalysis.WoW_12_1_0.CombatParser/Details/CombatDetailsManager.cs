using CombatAnalysis.WoW_12_1_0.CombatParser.Core;
using CombatAnalysis.WoW_12_1_0.CombatParser.Entities;
using CombatAnalysis.WoW_12_1_0.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW_12_1_0.CombatParser.Enums;
using System.Collections.Concurrent;
using System.Globalization;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

internal class CombatDetailsManager(string[] playersId, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
{
    private readonly string[] _playersId = playersId;
    private readonly DateTimeOffset _combatStarted = combatStarted;
    private readonly DateTimeOffset _combatFinished = combatFinished;

    public void GetSummonUnit(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (!units.TryGetValue(combatDataLine[6], out var _))
        {
            units.TryAdd(combatDataLine[6], new CombatUnit
            {
                GameId = combatDataLine[6],
                Username = combatDataLine[7],
                CreatorGameId = combatDataLine[2],
                UnitType = combatDataLine[^1],
            });
        }
    }

    public UnitHealth? GetUnitDeathHealth(string[] combatDataLine, ConcurrentDictionary<string, List<UnitHealth>> units)
    {
        if (!units.TryGetValue(combatDataLine[6], out var _))
        {
            units.TryAdd(combatDataLine[6], []);
        }

        var health = new UnitHealth
        {
            CreatorGameId = combatDataLine[6],
            CurrentHealth = 0,
            MaxHealth = 0,
            Time = GetTimeFromStart(combatDataLine[0]),
            IsDead = true,
        };

        return health;
    }

    public UnitHealth? GetUnitHealth(string[] combatDataLine, ConcurrentDictionary<string, List<UnitHealth>> units)
    {
        if (!int.TryParse(combatDataLine[15], out var currentHealth) || !int.TryParse(combatDataLine[16], out var maxHealth))
        {
            return null;
        }

        var creatorId = maxHealth > 100 ? combatDataLine[6] : combatDataLine[2];
        if (!units.TryGetValue(creatorId, out var _))
        {
            units.TryAdd(creatorId, []);
        }

        var time = GetTimeFromStart(combatDataLine[0]);
        var health = new UnitHealth
        {
            CreatorGameId = creatorId,
            CurrentHealth = currentHealth,
            MaxHealth = maxHealth,
            Time = time,
            IsDead = currentHealth == 0,
        };

        return health;
    }

    public void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, List<CombatPlayerAura>> auras, List<string> petsId)
    {
        if (!auras.TryGetValue(combatDataLine[2], out var combatPlayerAuras))
        {
            combatPlayerAuras = [];
            auras.TryAdd(combatDataLine[2], combatPlayerAuras);
        }

        var gameSpellId = int.Parse(combatDataLine[10]);
        if (combatDataLine[1].Equals(CombatLogKeyWords.AuraApplied) || combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose))
        {
            var aura = CreateCombatAura(gameSpellId, combatDataLine, combatDataLine[0], string.Empty, petsId);
            if (combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose) && int.TryParse(combatDataLine[^1], out var stacks))
            {
                aura.Stacks = stacks;
            }

            combatPlayerAuras.Add(aura);
        }
        else
        {
            RemoveAura(gameSpellId, combatDataLine, combatPlayerAuras, petsId);
        }
    }

    public void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, List<UnitCast>> casts)
    {
        if (!casts.TryGetValue(combatDataLine[2], out var combatPlayerCasts))
        {
            combatPlayerCasts = [];
            casts.TryAdd(combatDataLine[2], combatPlayerCasts);
        }

        var gameSpellId = int.Parse(combatDataLine[10]);
        if (combatDataLine[1].Equals(CombatLogKeyWords.SpellCastStart))
        {
            var newCast = CreateUnitCast(gameSpellId, combatDataLine, combatDataLine[0], combatDataLine[0], false, combatDataLine[1].Equals(CombatLogKeyWords.SpellCastSuccess));
            combatPlayerCasts.Add(newCast);
        }
        else
        {
            FinishCast(gameSpellId, combatDataLine, combatPlayerCasts, combatDataLine[1].Equals(CombatLogKeyWords.SpellCastSuccess));
        }
    }

    public void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, List<UnitPosition>> positions, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (combatDataLine.Length <= 25)
        {
            return;
        }

        var positionOwnerId = combatDataLine[2];
        var positionOwner = combatDataLine[3];
        if (!positions.TryGetValue(positionOwnerId, out var collection))
        {
            collection = [];
            positions.TryAdd(positionOwnerId, collection);
        }

        var pos1Index = 26;
        var pos2Index = 27;

        if (combatDataLine[1].Equals(CombatLogKeyWords.SwingDamage)
            || combatDataLine[1].Equals(CombatLogKeyWords.SwingDamageLanded))
        {
            pos1Index = 23;
            pos2Index = 24;
        }

        if (double.TryParse(combatDataLine[pos1Index], out var positionX)
            && double.TryParse(combatDataLine[pos2Index], out var positionY))
        {
            if (!units.TryGetValue(positionOwnerId, out var _))
            {
                units.TryAdd(positionOwnerId, new CombatUnit
                {
                    GameId = positionOwnerId,
                    Username = positionOwner,
                });
            }

            var position = new UnitPosition
            {
                CreatorGameId = positionOwnerId,
                X = positionX,
                Y = positionY,
                Time = GetTimeFromStart(combatDataLine[0])
            };

            collection.Add(position);
        }
    }

    public (string, DamageDone?) GetPlayerDamageDone(string[] combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[2]))
            || _playersId.Any(playerId => playerId.Equals(combatDataLine[6]))
            || combatDataLine[6].Contains("0000000000000000"))
        {
            return (string.Empty, null);
        }

        var damageDone = GetDamageDone(combatDataLine, false);

        return (combatDataLine[2], damageDone);
    }

    public (string, DamageDone?) GetPetsDamageDone(string[] combatDataLine, Dictionary<string, List<string>> petsId)
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

    public (string, HealDone?) GetHealDone(string[] combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[2])))
        {
            return (string.Empty, null);
        }

        if (!int.TryParse(combatDataLine[^4], out var value) || !int.TryParse(combatDataLine[^3], out var overheal))
        {
            return (string.Empty, null);
        }

        var isCrit = combatDataLine[^1].Contains(CombatLogKeyWords.IsCrit);

        var healDone = new HealDone
        {
            GameSpellId = int.Parse(combatDataLine[10]),
            Spell = combatDataLine[11].Trim('"'),
            Value = value,
            Overheal = overheal,
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            IsCrit = isCrit
        };

        return (combatDataLine[2], healDone);
    }

    public (string, HealDone?) GetAbsorb(string[] combatDataLine)
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
            Creator = combatDataLine[^8].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
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

    public (string, DamageTaken?) GetDamageTaken(string[] combatDataLine)
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

        if (!int.TryParse(combatDataLine[^10], out var value))
        {
            return (string.Empty, null);
        }

        var isAutoAttack = false;
        var spell = string.Empty;
        if (combatDataLine[1].Equals(CombatLogKeyWords.SwingDamage) || combatDataLine[1].Equals(CombatLogKeyWords.SwingMissed))
        {
            spell = CombatLogKeyWords.Melee;
            isAutoAttack = true;
        }
        else
        {
            spell = combatDataLine[11].Trim('"');
        }

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
            GameSpellId = isAutoAttack ? 0 : int.Parse(combatDataLine[10]),
            Spell = spell,
            Value = value,
            ActualValue = value + absorb,
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = enemy.Trim('"'),
            Target = combatDataLine[7].Trim('"'),
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

    public (string, ResourceRecovery?) GetResourceRecovery(string[] combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[6])))
        {
            return (string.Empty, null);
        }

        var energyRecovery = new ResourceRecovery
        {
            GameSpellId = int.Parse(combatDataLine[10]),
            Spell = combatDataLine[11].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"')
        };

        if (int.TryParse(combatDataLine[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery))
        {
            energyRecovery.Value = amoutOfResourcesRecovery;
        }

        return (combatDataLine[6], energyRecovery);
    }

    public (string, CombatPlayerDeath?) GetPlayerDeath(string[] combatDataLine)
    {
        if (!_playersId.Any(playerId => playerId.Equals(combatDataLine[6])))
        {
            return (string.Empty, null);
        }

        var userDeath = new CombatPlayerDeath
        {
            Username = combatDataLine[7].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
        };

        return (combatDataLine[6], userDeath);
    }

    private DamageDone GetDamageDone(string[] combatDataLine, bool isPet, string spell = "")
    {
        var isAutoAttack = false;
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spell += CombatLogKeyWords.Melee;
            isAutoAttack = true;
        }
        else
        {
            spell += combatDataLine[11].Trim('"');
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

        var isCrit = string.Equals(isAutoAttack ? combatDataLine[^3] : combatDataLine[^4], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

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

        var isSingleTarget = 
            isAutoAttack 
            || (isPeriodicDamage 
            || (string.Equals(combatDataLine[^1], CombatLogKeyWords.IsSingleTarget + "\r", StringComparison.OrdinalIgnoreCase)));

        var damageDone = new DamageDone
        {
            GameSpellId = isAutoAttack ? 0 : int.Parse(combatDataLine[10]),
            Spell = spell,
            Time = GetTimeFromStart(combatDataLine[0]),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            IsTargetBoss = combatDataLine[6].Contains(CombatLogKeyWords.Boss),
            DamageType = (int)damageType,
            IsPeriodicDamage = isPeriodicDamage,
            IsSingleTarget = isSingleTarget,
            IsPet = isPet,
        };

        if (int.TryParse(isAutoAttack ? combatDataLine[^10] : combatDataLine[^11], out var value))
        {
            damageDone.Value = value;
        }

        return damageDone;
    }

    private CombatPlayerAura CreateCombatAura(int gameSpellId, string[] combatDataLine, string startTimeAura, string finishTimeAura, List<string> petsId)
    {
        var startTime = GetTimeFromStart(startTimeAura);
        var finishTime = GetTimeFromStart(finishTimeAura);
        var auraType = SelectAuraType(combatDataLine);
        var auraCreatorType = SelectAuraCreatorType(combatDataLine[2], petsId);

        var aura = new CombatPlayerAura
        {
            GameAuraId = int.Parse(combatDataLine[10]),
            Name = combatDataLine[11].Trim('"'),
            Creator = combatDataLine[3].Trim('"'),
            Target = combatDataLine[7].Trim('"'),
            StartTime = startTime,
            FinishTime = finishTime,
            AuraCreatorType = (int)auraCreatorType,
            AuraType = (int)auraType
        };

        return aura;
    }

    private UnitCast CreateUnitCast(int gameSpellId, string[] combatDataLine, string startTimeCast, string finishTimeCast, bool isImmediatly, bool isSuccess)
    {
        var startTime = GetTimeFromStart(startTimeCast);
        var finishTime = GetTimeFromStart(finishTimeCast);

        var cast = new UnitCast
        {
            CreatorGameId = combatDataLine[2],
            GameSpellId = gameSpellId,
            Spell = combatDataLine[11].Trim('"'),
            Time = startTime,
            FinishTime = finishTime,
            TargetGameId = combatDataLine[7].Equals(CombatLogKeyWords.NullValue, StringComparison.OrdinalIgnoreCase) ? null : combatDataLine[6],
            IsImmediatly = isImmediatly,
            IsSuccess = isSuccess,
        };

        return cast;
    }

    private void RemoveAura(int gameSpellId, string[] combatDataLine, List<CombatPlayerAura> combatPlayerAuras, List<string> petsId)
    {
        var aura = combatPlayerAuras
            .FirstOrDefault(x => x.GameAuraId == gameSpellId);
        if (aura != null)
        {
            aura.FinishTime = GetTimeFromStart(combatDataLine[0]);
        }
    }

    private void FinishCast(int gameSpellId, string[] combatDataLine, List<UnitCast> combatPlayerCasts, bool isSuccess)
    {
        var lastStartedCast = combatPlayerCasts
            .LastOrDefault(x => x.GameSpellId == gameSpellId && !x.IsImmediatly);
        if (lastStartedCast != null)
        {
            lastStartedCast.FinishTime = GetTimeFromStart(combatDataLine[0]);
            lastStartedCast.TargetGameId = combatDataLine[7].Equals(CombatLogKeyWords.NullValue, StringComparison.OrdinalIgnoreCase) ? null : combatDataLine[6];
            lastStartedCast.IsSuccess = isSuccess;
        }
        else
        {
            var instaCast = CreateUnitCast(gameSpellId, combatDataLine, combatDataLine[0], combatDataLine[0], true, isSuccess);
            combatPlayerCasts.Add(instaCast);
        }
    }

    private static AuraType SelectAuraType(string[] combatDataLine)
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
        if (DateTimeOffset.TryParse(time, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startTime))
        {
            var timeFromStart = startTime - _combatStarted;

            return timeFromStart < TimeSpan.Zero ? TimeSpan.Zero : timeFromStart;
        }

        return TimeSpan.Zero;
    }
}