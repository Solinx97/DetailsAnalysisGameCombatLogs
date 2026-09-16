using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Details;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Details;

public abstract class CombatDetails(ICombatParserHelper combatParserHelper, ILogger logger, ConcurrentDictionary<string, Unit> units)
{
    protected readonly ICombatParserHelper _combatParserHelper = combatParserHelper;

    protected readonly string[] _dieds =
    [
        CombatLogKeyWords.UnitDied,
    ];
    protected readonly string[] _auras =
    [
        CombatLogKeyWords.AuraApplied,
        CombatLogKeyWords.AuraRemoved,
        CombatLogKeyWords.AuraAppliedDose,
        CombatLogKeyWords.AuraRemovedDose,
    ];
    protected readonly string[] _casts =
    [
        CombatLogKeyWords.SpellCastStart,
        CombatLogKeyWords.SpellCastSuccess,
        CombatLogKeyWords.SpellCastFailed,
    ];
    protected readonly string[] _positions =
    [
        CombatLogKeyWords.SpellCastSuccess,
    ];
    protected readonly string[] _damageHealth =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.RangeDamage,
    ];
    protected readonly string[] _healHealth =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
    ];
    protected readonly string[] _damageVariations =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
    ];
    protected readonly string[] _healVariations =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
    ];
    protected readonly string[] _absorbVariations =
    [
        CombatLogKeyWords.SpellAbsorbed,
    ];
    protected readonly string[] _resourceVariations =
    [
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    ];

    public ILogger Logger { get; private set; } = logger;

    #region Details collections

    public ConcurrentDictionary<string, Unit> Units { get; protected set; } = units;

    #endregion

    public void Clear()
    {
        foreach (var unit in Units)
        {
            unit.Value.DamageDones.Clear();
            unit.Value.DamageTakens.Clear();
            unit.Value.HealDones.Clear();
            unit.Value.ResourceRecoveries.Clear();
        }

        Units.Clear();
    }

    public virtual void Calculate(string[] combatData, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(combatData, nameof(combatData));
            ArgumentOutOfRangeException.ThrowIfZero(combatData.Length);

            foreach (var combatDataLine in combatData)
            {
                Parse(combatDataLine, combatStarted, combatFinished);
            }
        }
        catch (ArgumentNullException ex)
        {
            Logger.LogError("Some argument was null: {Param}", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Logger.LogError("Some argument out of valid range: {Param}", ex.ParamName);
        }
    }

    protected abstract CombatDetailsManager CreateCombatDetailsManager(DateTimeOffset combatStarted, DateTimeOffset combatFinished);

    private void Parse(string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasCasts = _casts.Any(combatDataLine.Contains);
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasDamageHealth = _damageHealth.Any(combatDataLine.Contains);
        var hasHealHealth = _healHealth.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasCasts && !hasPositions && !hasDieds && !hasAuras
            && !hasDamageHealth !&& hasHealHealth && !hasDamage && !hasHeal && !hasAbsorb && !hasResources)
        {
            return;
        }

        var splitCombatData = _combatParserHelper.SplitCombatData(combatDataLine);
        var combatDetailsManager = CreateCombatDetailsManager(combatStarted, combatFinished);

        Parallel.Invoke(
                () =>
                {
                    if (hasCasts)
                    {
                        combatDetailsManager.GetCasts(splitCombatData, Units);
                    }
                },
                () =>
                {
                    if (hasPositions)
                    {
                        combatDetailsManager.GetPosition(splitCombatData, Units);
                    }
                },
                () =>
                {
                    if (hasDamageHealth)
                    {
                        combatDetailsManager.GetHealth(splitCombatData, Units, UnitHealthStatus.Decrease);
                    }
                    else if (hasHealHealth)
                    {
                        combatDetailsManager.GetHealth(splitCombatData, Units, UnitHealthStatus.Increase);
                    }
                },
                () =>
                {
                    CalculateGeneral(combatDataLine, combatDetailsManager, splitCombatData);
                }
            );
    }

    protected void CalculateGeneral(string combatDataLine, ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (hasDieds)
        {
            combatDetailsManager.AddUnitDeath(splitCombatData, Units);
        }
        else if (hasAuras)
        {
            combatDetailsManager.GetAuras(splitCombatData, units);
        }
        else if (hasHeal)
        {
            combatDetailsManager.GetHealDone(splitCombatData, Units);
        }
        else if (hasAbsorb)
        {
            combatDetailsManager.GetAbsorb(splitCombatData, Units);
        }
        else if (hasDamage)
        {
            combatDetailsManager.GetDamageDone(splitCombatData, Units);
        }
        else if (hasResources)
        {
            combatDetailsManager.GetResourceRecovery(splitCombatData, Units);
        }
    }
}
