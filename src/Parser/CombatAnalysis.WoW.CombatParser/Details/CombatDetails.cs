using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Details;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
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

    public ConcurrentDictionary<string, List<CombatPlayerAura>> Auras { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ICombatPlayerResourceRefs>> DamageDones { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ICombatPlayerResourceRefs>> HealDones { get; private set; } = [];

    public Dictionary<string, List<HealDoneGeneral>> HealDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ICombatPlayerResourceRefs>> DamageTakens { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageTakenGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ICombatPlayerResourceRefs>> ResourcesRecoveries { get; private set; } = [];

    public Dictionary<string, List<ResourceRecoveryGeneral>> ResourcesRecoveryGenerals { get; private set; } = [];

    #endregion

    public void Clear()
    {
        ClearNested(Auras);
        ClearNested(DamageDones);
        ClearNested(HealDones);
        ClearNested(DamageTakens);
        ClearNested(ResourcesRecoveries);

        DamageDoneGenerals.Clear();
        HealDoneGenerals.Clear();
        DamageTakenGenerals.Clear();
        ResourcesRecoveryGenerals.Clear();

        Units.Clear();
    }

    public virtual void Calculate(string[] playersId, string[] combatData, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(playersId, nameof(playersId));
            ArgumentNullException.ThrowIfNull(combatData, nameof(combatData));
            ArgumentOutOfRangeException.ThrowIfZero(playersId.Length);
            ArgumentOutOfRangeException.ThrowIfZero(combatData.Length);

            for (int i = 0; i < playersId.Length; i++)
            {
                PrepareCollections(playersId[i]);
            }

            foreach (var combatDataLine in combatData)
            {
                Parse(playersId, combatDataLine, combatStarted, combatFinished);
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

    protected void PrepareCollections(string playersd)
    {
        Auras.TryAdd(playersd, []);

        DamageDones.TryAdd(playersd, []);
        HealDones.TryAdd(playersd, []);
        DamageTakens.TryAdd(playersd, []);
        ResourcesRecoveries.TryAdd(playersd, []);
    }

    protected abstract void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished);

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

    protected virtual void CalculateDamageTaken(ICombatDetailsManager combatDetailsManager, string[] splitCombatData, string[] playersId)
    {
        var damageTaken = combatDetailsManager.GetDamageDone(splitCombatData, Units);
        if (damageTaken != null && damageTaken.Target.GameId.Contains("Player"))
        {
            if (DamageTakens.TryGetValue(damageTaken.Target.GameId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), damageTaken);
            }
            else
            {
                var newDictionary = new ConcurrentDictionary<string, ICombatPlayerResourceRefs>();
                newDictionary.TryAdd(Guid.NewGuid().ToString(), damageTaken);
                DamageTakens.TryAdd(damageTaken.Target.GameId, newDictionary);
            }
        }
    }

    protected void CalculateGeneral(string combatDataLine, ICombatDetailsManager combatDetailsManager, string[] splitCombatData, string[] playersId)
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
            var units = Units.Select(x => x.Value).ToList();
            combatDetailsManager.GetAuras(splitCombatData, Auras, units);
        }
        else if (hasHeal)
        {
            var healDone = combatDetailsManager.GetHealDone(splitCombatData, Units);
            GroupUnits(healDone, HealDones);
        }
        else if (hasAbsorb)
        {
            var absorb = combatDetailsManager.GetAbsorb(splitCombatData, Units);
            GroupUnits(absorb, HealDones);
        }
        else if (hasDamage)
        {
            var damageDone = combatDetailsManager.GetDamageDone(splitCombatData, Units);
            GroupUnits(damageDone, DamageDones);
        }
        else if (hasResources)
        {
            var resourceRecovery = combatDetailsManager.GetResourceRecovery(splitCombatData, Units);
            GroupUnits(resourceRecovery, ResourcesRecoveries);
        }
    }

    private void GroupUnits<TModel>(TModel entity, ConcurrentDictionary<string, ConcurrentDictionary<string, ICombatPlayerResourceRefs>> targetDictionary)
        where TModel : ICombatPlayerResourceRefs
    {
        var selectedId = entity.Creator.GameId;
        var playerCreature = Units
            .FirstOrDefault(x => x.Value.CreatorGameId != null && x.Value.GameId == entity.Creator.GameId).Value;
        if (playerCreature != null)
        {
            entity.Spell = $"{playerCreature.Name} - {entity.Spell}";
            selectedId = playerCreature.CreatorGameId!;
        }

        if (targetDictionary.TryGetValue(selectedId, out var collection))
        {
            collection.TryAdd(Guid.NewGuid().ToString(), entity);
        }
        else
        {
            var newDictionary = new ConcurrentDictionary<string, ICombatPlayerResourceRefs>();
            newDictionary.TryAdd(Guid.NewGuid().ToString(), entity);
            targetDictionary.TryAdd(selectedId, newDictionary);
        }
    }

    private static void ClearNested<T>(ConcurrentDictionary<string, List<T>> source)
    {
        foreach (var item in source.Values)
        {
            item.Clear();
        }

        source.Clear();
    }

    private static void ClearNested<T>(ConcurrentDictionary<string, ConcurrentDictionary<string, T>> source)
    {
        foreach (var item in source.Values)
        {
            item.Clear();
        }

        source.Clear();
    }
}
