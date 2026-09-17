using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Details;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using System.Collections.Concurrent;
using System.Globalization;

namespace CombatAnalysis.WoW.CombatParser.Details;

public abstract class CombatDetailsManager(ICombatParserHelper combatParserHelper, DateTimeOffset combatStarted, DateTimeOffset combatFinished) 
    : ICombatDetailsManager
{
    private readonly ICombatParserHelper _combatParserHelper = combatParserHelper;
    private readonly DateTimeOffset _combatStarted = combatStarted;
    private readonly DateTimeOffset _combatFinished = combatFinished;

    public void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var ownerId = combatDataLine[6];
        if (!units.TryGetValue(ownerId, out var unit))
        {
            return;
        }

        var gameSpellId = int.Parse(combatDataLine[10]);
        if (combatDataLine[1].Equals(CombatLogKeyWords.AuraApplied) || combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose))
        {
            var aura = CreateCombatAura(combatDataLine, combatDataLine[0], string.Empty, units);
            if (combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose) && int.TryParse(combatDataLine[^1], out var stacks))
            {
                aura.Stacks = stacks;
            }

            unit.Auras.Add(aura);
        }
        else
        {
            RemoveAura(gameSpellId, combatDataLine, unit.Auras);
        }
    }

    public void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        if (!units.TryGetValue(combatDataLine[2], out var unit))
        {
            return;
        }

        var gameSpellId = int.Parse(combatDataLine[10]);
        if (combatDataLine[1].Equals(CombatLogKeyWords.SpellCastStart))
        {
            var unitCast = CreateUnitCast(gameSpellId, combatDataLine, combatDataLine[0], combatDataLine[0], false, combatDataLine[1].Equals(CombatLogKeyWords.SpellCastSuccess));
            unit.UnitCasts.Add(unitCast);
        }
        else
        {
            FinishCast(gameSpellId, combatDataLine, unit.UnitCasts, combatDataLine[1].Equals(CombatLogKeyWords.SpellCastSuccess));
        }
    }

    public void GetHealth(string[] combatDataLine, ConcurrentDictionary<string, Unit> units, UnitHealthStatus status)
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

    public void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var positionOwnerId = combatDataLine[2];
        if (combatDataLine.Length <= 25 || !units.TryGetValue(positionOwnerId, out var unit))
        {
            return;
        }

        var pos1Index = 26;
        var pos2Index = 27;

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

    public void GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        int.TryParse(combatDataLine[^4], out var value);
        int.TryParse(combatDataLine[^3], out var overheal);

        var isCrit = combatDataLine[^1].Contains(CombatLogKeyWords.IsCrit);
        var healDone = new HealDone
        {
            GameSpellId = int.Parse(combatDataLine[10]),
            Spell = combatDataLine[11].Trim('"'),
            Value = value,
            Overheal = overheal,
            Time = GetTimeFromStart(combatDataLine[0]),
            ModificationType = isCrit ? (int)ModificationType.Crit : (int)ModificationType.Normal,
        };

        ApplyUnits(combatDataLine, healDone, units, 2, 6);

        if (units.TryGetValue(healDone.CreatorGameId, out var creatorUnit))
        {
            creatorUnit.HealDones.Add(healDone);
        }
    }

    public abstract void GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, Unit> units);

    public void GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var resourceRecovery = new ResourceRecovery
        {
            GameSpellId = int.Parse(combatDataLine[10]),
            Spell = combatDataLine[11].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
        };

        if (int.TryParse(combatDataLine[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery))
        {
            resourceRecovery.Value = amoutOfResourcesRecovery;
        }

        ApplyUnits(combatDataLine, resourceRecovery, units, 2, 6);

        if (units.TryGetValue(resourceRecovery.CreatorGameId, out var creatorUnit))
        {
            creatorUnit.ResourceRecoveries.Add(resourceRecovery);
        }
    }

    public void AddUnitDeath(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        if (units.TryGetValue(combatDataLine[6], out var unit) && unit.UnitHealthes.Count > 0)
        {
            AddUnitHealth(unit, combatDataLine[6], 0, unit.UnitHealthes[^1].MaxHealth, combatDataLine[0], UnitHealthStatus.Dead);
        }
    }

    public void GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, Unit> units)
    {
        var spell = string.Empty;
        var isAutoAttack = false;
        if (string.Equals(combatDataLine[1] + ',', CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spell += CombatLogKeyWords.Melee;
            isAutoAttack = true;
        }
        else
        {
            spell += combatDataLine[11].Trim('"');
        }

        int.TryParse(isAutoAttack ? combatDataLine[^10] : combatDataLine[^11], out var value);

        var isAbsorbed = string.Equals(combatDataLine[^4], CombatLogKeyWords.Absorb, StringComparison.OrdinalIgnoreCase);
        var hasTypeOfTarget = string.Equals(combatDataLine[^1], CombatLogKeyWords.IsSingleTarget + '\r', StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[^1], CombatLogKeyWords.IsAOETarget + '\r', StringComparison.OrdinalIgnoreCase);

        if (!isAbsorbed && hasTypeOfTarget)
        {
            isAbsorbed = string.Equals(combatDataLine[^5], CombatLogKeyWords.Absorb, StringComparison.OrdinalIgnoreCase);
        }

        var damageType = GetDamageType(combatDataLine);
        var damageModificationType = GetDamageModification(combatDataLine, isAbsorbed);
        var damageDone = new DamageDone
        {
            GameSpellId = isAutoAttack ? 0 : int.Parse(combatDataLine[10]),
            Spell = spell,
            Time = GetTimeFromStart(combatDataLine[0]),
            DamageType = (int)damageType,
            ModificationType = (int)damageModificationType,
        };

        ApplyUnits(combatDataLine, damageDone, units, 2, 6);
        ApplyDamageModification(combatDataLine, value, isAbsorbed, hasTypeOfTarget, damageDone);

        if (units.TryGetValue(damageDone.CreatorGameId, out var creatorUnit))
        {
            creatorUnit.DamageDones.Add(damageDone);
        }
        if (units.TryGetValue(damageDone.TargetGameId, out var targetUnit))
        {
            targetUnit.DamageTakens.Add(damageDone);
        }
    }

    private void AddUnitHealth(Unit unit, string ownerId, long currentHealth, long maxHealth, string time, UnitHealthStatus status)
    {
        var health = new UnitHealth
        {
            OwnerGameId = ownerId,
            CurrentHealth = currentHealth,
            MaxHealth = maxHealth,
            Status = (int)status,
            Time = GetTimeFromStart(time)
        };

        unit.UnitHealthes.Add(health);
    }

    private UnitAura CreateCombatAura(string[] combatDataLine, string startTimeAura, string finishTimeAura, ConcurrentDictionary<string, Unit> units)
    {
        var startTime = GetTimeFromStart(startTimeAura);
        var finishTime = GetTimeFromStart(finishTimeAura);
        var auraType = SelectAuraType(combatDataLine);
        var auraCreatorType = SelectAuraCreatorType(combatDataLine[2], units);

        var aura = new UnitAura
        {
            GameAuraId = int.Parse(combatDataLine[10]),
            Name = combatDataLine[11].Trim('"'),
            TargetGameId = combatDataLine[6],
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
            OwnerGameId = combatDataLine[2],
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

    private void RemoveAura(int gameSpellId, string[] combatDataLine, List<UnitAura> unitAuras)
    {
        var aura = unitAuras
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

    private static AuraCreatorType SelectAuraCreatorType(string creatorId, ConcurrentDictionary<string, Unit> units)
    {
        if (creatorId.Contains(CombatLogKeyWords.Player))
        {
            return AuraCreatorType.Player;
        }
        else if (creatorId.Contains(CombatLogKeyWords.Pet))
        {
            return AuraCreatorType.Pet;
        }
        else if (units.Any(x => x.Value.CreatorGameId != null && x.Value.CreatorGameId.Contains(creatorId)))
        {
            return AuraCreatorType.AllyCreature;
        }
        else
        {
            return AuraCreatorType.EnemyCreature;
        }
    }

    protected TimeSpan GetTimeFromStart(string time)
    {
        if (DateTimeOffset.TryParse(time, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startTime))
        {
            var timeFromStart = startTime - _combatStarted;

            return timeFromStart < TimeSpan.Zero ? TimeSpan.Zero : timeFromStart;
        }

        return TimeSpan.Zero;
    }

    private static ModificationType GetDamageModification(string[] combatDataLine, bool isAbsorbed)
    {
        var isCrushing = string.Equals(combatDataLine[^1], CombatLogKeyWords.IsCrushing, StringComparison.OrdinalIgnoreCase);

        var index = -1;
        if (isAbsorbed)
        {
            index = combatDataLine.Length - 4;
        }
        else if (string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            index = combatDataLine.Length - 3;
        }
        else if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            index = combatDataLine.Length - 2;
        }

        var isCrit = string.Equals(combatDataLine[^4], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var isParry = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
        var isDodge = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
        var isMiss = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        var isResist = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
        var isImmune = index >= 0 && string.Equals(combatDataLine[index], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);

        var damageModificationType = isCrushing ? ModificationType.Crushing : ModificationType.Normal;
        damageModificationType = isCrit ? ModificationType.Crit : damageModificationType;
        damageModificationType = isParry ? ModificationType.Parry : damageModificationType;
        damageModificationType = isDodge ? ModificationType.Dodge : damageModificationType;
        damageModificationType = isMiss ? ModificationType.Miss : damageModificationType;
        damageModificationType = isResist ? ModificationType.Resist : damageModificationType;
        damageModificationType = isImmune ? ModificationType.Immune : damageModificationType;
        damageModificationType = isAbsorbed ? ModificationType.Absorb : damageModificationType;

        return damageModificationType;
    }

    private static DamageType GetDamageType(string[] combatDataLine)
    {
        var damageType = DamageType.ST;
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellPeriodicDamage, StringComparison.OrdinalIgnoreCase))
        {
            damageType = DamageType.Periodic;
        }
        else if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellDamage, StringComparison.OrdinalIgnoreCase)
            && string.Equals(combatDataLine[^1], CombatLogKeyWords.IsSingleTarget, StringComparison.OrdinalIgnoreCase))
        {
            damageType = DamageType.ST;
        }
        else if (string.Equals(combatDataLine[1], CombatLogKeyWords.SpellDamage, StringComparison.OrdinalIgnoreCase)
            && string.Equals(combatDataLine[^1], CombatLogKeyWords.IsAOETarget, StringComparison.OrdinalIgnoreCase))
        {
            damageType = DamageType.AOE;
        }

        return damageType;
    }

    private static void ApplyDamageModification(string[] combatDataLine, int value, bool isAbsorbed, bool hasTypeOfTarget, DamageDone damageDone)
    {
        int realDamage = 0, overkill = -1, mitigated = 0, absorb = 0, blocked = 0, resist = 0;
        if (isAbsorbed)
        {
            var absorbIndex = hasTypeOfTarget ? combatDataLine.Length - 3 : combatDataLine.Length - 2;
            var realDamageIndex = hasTypeOfTarget ? combatDataLine.Length - 2 : combatDataLine.Length - 1;

            int.TryParse(combatDataLine[absorbIndex], out absorb);
            int.TryParse(combatDataLine[realDamageIndex], out realDamage);

            mitigated = realDamage - value - absorb;
        }
        else if (!string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
        {
            int.TryParse(combatDataLine[^5], out absorb);
            int.TryParse(combatDataLine[^6], out blocked);
            int.TryParse(combatDataLine[^7], out resist);

            if (string.Equals(combatDataLine[1] + ',', CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(combatDataLine[^5], out absorb);
                int.TryParse(combatDataLine[^8], out overkill);
                int.TryParse(combatDataLine[^9], out realDamage);
            }
            else
            {
                int.TryParse(combatDataLine[^9], out overkill);
                int.TryParse(combatDataLine[^8], out realDamage);
            }

            mitigated = realDamage - value - absorb;
        }

        damageDone.Value = overkill < 0 ? value : value - overkill;
        damageDone.Resisted = resist;
        damageDone.Absorbed = absorb;
        damageDone.Blocked = blocked;
        damageDone.RealDamage = realDamage;
        damageDone.Overkill = overkill;
        damageDone.Mitigated = mitigated < 0 ? 0 : mitigated;
    }

    protected void ApplyUnits(string[] combatDataLine, ICombatUnitRefs unitData, ConcurrentDictionary<string, Unit> units, int creatorGameIdIndex, int targetGameIdIndex)
    {
        if (!units.TryGetValue(combatDataLine[creatorGameIdIndex], out var creatorUnit))
        {
            creatorUnit = _combatParserHelper.ParseUnits(units, combatDataLine[creatorGameIdIndex], combatDataLine[creatorGameIdIndex + 1], combatDataLine[creatorGameIdIndex + 2]);
        }
        if (!units.TryGetValue(combatDataLine[targetGameIdIndex], out var targetUnit))
        {
            targetUnit = _combatParserHelper.ParseUnits(units, combatDataLine[targetGameIdIndex], combatDataLine[targetGameIdIndex + 1], combatDataLine[targetGameIdIndex + 2]);
        }

        unitData.CreatorGameId = creatorUnit.GameId;
        unitData.TargetGameId = targetUnit.GameId;
    }
}