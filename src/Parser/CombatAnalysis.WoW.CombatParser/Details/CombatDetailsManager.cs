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

public abstract class CombatDetailsManager(ICombatParserHelper combatParserHelper, string[] playersId, DateTimeOffset combatStarted, DateTimeOffset combatFinished) 
    : ICombatDetailsManager
{
    private readonly string[] _playersId = playersId;
    private readonly ICombatParserHelper _combatParserHelper = combatParserHelper;
    private readonly DateTimeOffset _combatStarted = combatStarted;
    private readonly DateTimeOffset _combatFinished = combatFinished;

    public void GetAuras(string[] combatDataLine, ConcurrentDictionary<string, List<CombatPlayerAura>> auras, List<CombatUnit> summonedCreatures)
    {
        if (!auras.TryGetValue(combatDataLine[2], out var combatPlayerAuras))
        {
            combatPlayerAuras = [];
            auras.TryAdd(combatDataLine[2], combatPlayerAuras);
        }

        var gameSpellId = int.Parse(combatDataLine[10]);
        if (combatDataLine[1].Equals(CombatLogKeyWords.AuraApplied) || combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose))
        {
            var aura = CreateCombatAura(gameSpellId, combatDataLine, combatDataLine[0], string.Empty, summonedCreatures);
            if (combatDataLine[1].Equals(CombatLogKeyWords.AuraAppliedDose) && int.TryParse(combatDataLine[^1], out var stacks))
            {
                aura.Stacks = stacks;
            }

            combatPlayerAuras.Add(aura);
        }
        else
        {
            RemoveAura(gameSpellId, combatDataLine, combatPlayerAuras);
        }
    }

    public void GetCasts(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
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

    public void GetPosition(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (combatDataLine.Length <= 25 || !units.TryGetValue(combatDataLine[2], out var unit))
        {
            return;
        }

        var positionOwnerId = combatDataLine[2];
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
            var position = new UnitPosition
            {
                CreatorGameId = positionOwnerId,
                X = positionX,
                Y = positionY,
                Time = GetTimeFromStart(combatDataLine[0])
            };

            unit.UnitPositions.Add(position);
        }
    }

    public HealDone? GetHealDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (!int.TryParse(combatDataLine[^4], out var value) || !int.TryParse(combatDataLine[^3], out var overheal))
        {
            return null;
        }

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

        ApplyUnits(combatDataLine, healDone, units);

        return healDone;
    }

    public abstract HealDone? GetAbsorb(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units);

    public ResourceRecovery? GetResourceRecovery(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        var energyRecovery = new ResourceRecovery
        {
            GameSpellId = int.Parse(combatDataLine[10]),
            Spell = combatDataLine[11].Trim('"'),
            Time = GetTimeFromStart(combatDataLine[0]),
        };

        if (int.TryParse(combatDataLine[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery))
        {
            energyRecovery.Value = amoutOfResourcesRecovery;
        }

        ApplyUnits(combatDataLine, energyRecovery, units);

        return energyRecovery;
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

    public DamageDone? GetDamageDone(string[] combatDataLine, ConcurrentDictionary<string, CombatUnit> units)
    {
        var spell = string.Empty;
        var isAutoAttack = false;
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase)
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

        ApplyUnits(combatDataLine, damageDone, units);
        ApplyDamageModification(combatDataLine, value, isAbsorbed, hasTypeOfTarget, damageDone);
        AddDamageHealth(combatDataLine, damageDone);

        return damageDone;
    }

    private CombatPlayerAura CreateCombatAura(int gameSpellId, string[] combatDataLine, string startTimeAura, string finishTimeAura, List<CombatUnit> summonedCreatures)
    {
        var startTime = GetTimeFromStart(startTimeAura);
        var finishTime = GetTimeFromStart(finishTimeAura);
        var auraType = SelectAuraType(combatDataLine);
        var auraCreatorType = SelectAuraCreatorType(combatDataLine[2], summonedCreatures);

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

    private void RemoveAura(int gameSpellId, string[] combatDataLine, List<CombatPlayerAura> combatPlayerAuras)
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

    private static AuraCreatorType SelectAuraCreatorType(string creatorId, List<CombatUnit> summonedCreatures)
    {
        if (creatorId.Contains(CombatLogKeyWords.Player))
        {
            return AuraCreatorType.Player;
        }
        else if (creatorId.Contains(CombatLogKeyWords.Pet))
        {
            return AuraCreatorType.Pet;
        }
        else if (summonedCreatures.Any(x => x.CreatorGameId != null && x.CreatorGameId.Contains(creatorId)))
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

    private static void AddDamageHealth(string[] combatDataLine, DamageDone damageDone)
    {
        if (string.Equals(combatDataLine[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            //damageDone.Target.Health = -1;

            return;
        }

        var healthIndex = 12;
        var isSwingDamage = string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase)
                            || string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase);
        if (!isSwingDamage)
        {
            healthIndex = 15;
        }

        //damageDone.Target.Health = long.Parse(combatDataLine[healthIndex]);
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

            if (string.Equals(combatDataLine[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
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

    protected void ApplyUnits(string[] combatDataLine, ICombatUnitRefs unitData, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (!units.TryGetValue(combatDataLine[2], out var creatorUnit))
        {
            creatorUnit = _combatParserHelper.ParseUnits(units, combatDataLine[2], combatDataLine[3], combatDataLine[4]);
        }
        if (!units.TryGetValue(combatDataLine[6], out var targetUnit))
        {
            targetUnit = _combatParserHelper.ParseUnits(units, combatDataLine[6], combatDataLine[7], combatDataLine[8]);
        }

        unitData.Creator = creatorUnit;
        unitData.CreatorGameId = creatorUnit.GameId;
        unitData.Target = targetUnit;
        unitData.TargetGameId = targetUnit.GameId;
    }
}