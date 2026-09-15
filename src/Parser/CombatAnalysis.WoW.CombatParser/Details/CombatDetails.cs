using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
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
    protected readonly string[] _health =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.RangeDamage,
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

    protected abstract void Parse(string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished);

    protected virtual void CalculateCasts(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetCasts(splitCombatData, Units);
    }

    protected virtual void CalculatePositions(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetPosition(splitCombatData, Units);
    }

    protected virtual void CalculateHealthes(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetHealth(splitCombatData, Units);
    }

    protected virtual void CalculateDamageTaken(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        //var damageTaken = combatDetailsManager.GetDamageDone(splitCombatData, Units);
        //if (damageTaken != null && damageTaken.Target.GameId.Contains("Player"))
        //{
        //    if (DamageTakens.TryGetValue(damageTaken.Target.GameId, out var collection))
        //    {
        //        collection.TryAdd(Guid.NewGuid().ToString(), damageTaken);
        //    }
        //    else
        //    {
        //        var newDictionary = new ConcurrentDictionary<string, ICombatPlayerResourceRefs>();
        //        newDictionary.TryAdd(Guid.NewGuid().ToString(), damageTaken);
        //        DamageTakens.TryAdd(damageTaken.Target.GameId, newDictionary);
        //    }
        //}
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
