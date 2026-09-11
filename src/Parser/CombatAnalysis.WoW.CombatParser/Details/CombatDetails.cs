using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Details;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Details;

public abstract class CombatDetails(ICombatParserHelper combatParserHelper, ILogger logger, ConcurrentDictionary<string, CombatUnit> units)
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
    protected readonly string[] _healVariations =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
    ];
    protected readonly string[] _absorbVariations =
    [
        CombatLogKeyWords.SpellAbsorbed,
    ];
    protected readonly string[] _damageVariations =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamage + ',',
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
    ];
    protected readonly string[] _resourceVariations =
    [
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    ];

    public ILogger Logger { get; private set; } = logger;

    #region Details collections

    public ConcurrentDictionary<string, CombatUnit> Units { get; protected set; } = units;

    public ConcurrentDictionary<string, List<UnitCast>> UnitCasts { get; private set; } = [];

    public ConcurrentDictionary<string, List<UnitPosition>> UnitPositions { get; private set; } = [];

    public ConcurrentDictionary<string, List<CombatPlayerAura>> Auras { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, CombatPlayerDeath>> Deathes { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageDone>> DamageDones { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, HealDone>> HealDones { get; private set; } = [];

    public Dictionary<string, List<HealDoneGeneral>> HealDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageDone>> DamageTakens { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageTakenGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceRecovery>> ResourcesRecoveries { get; private set; } = [];

    public Dictionary<string, List<ResourceRecoveryGeneral>> ResourcesRecoveryGenerals { get; private set; } = [];

    #endregion

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
        UnitPositions.TryAdd(playersd, []);
        Deathes.TryAdd(playersd, []);
        UnitCasts.TryAdd(playersd, []);
        Auras.TryAdd(playersd, []);

        DamageDones.TryAdd(playersd, []);
        HealDones.TryAdd(playersd, []);
        DamageTakens.TryAdd(playersd, []);
        ResourcesRecoveries.TryAdd(playersd, []);
    }

    protected abstract void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished);

    protected virtual void CalculateCasts(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetCasts(splitCombatData, UnitCasts);
    }

    protected virtual void CalculatePositions(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetPosition(splitCombatData, UnitPositions);
    }

    protected virtual void CalculateDamageTaken(ICombatDetailsManager combatDetailsManager, string[] splitCombatData)
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
                var newDictionary = new ConcurrentDictionary<string, DamageDone>();
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
            var (playerId, death) = combatDetailsManager.GetPlayerDeath(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) && death != null && Deathes.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), death);
            }
        }
        else if (hasAuras)
        {
            var units = Units.Select(x => x.Value).ToList();
            combatDetailsManager.GetAuras(splitCombatData, Auras, units);
        }
        else if (hasHeal)
        {
            var (playerId, healDone) = combatDetailsManager.GetHealDone(splitCombatData, Units);
            if (!string.IsNullOrEmpty(playerId) && healDone != null && HealDones.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), healDone);
            }
        }
        else if (hasAbsorb)
        {
            var (playerId, absorb) = combatDetailsManager.GetAbsorb(splitCombatData, Units);
            if (absorb != null && HealDones.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), absorb);
            }
        }
        else if (hasDamage)
        {
            var damageDone = combatDetailsManager.GetDamageDone(splitCombatData, Units);
            if (damageDone != null)
            {
                AddDamageDone(damageDone, playersId);
            }
        }
        else if (hasResources)
        {
            var (playerId, resourceRecovery) = combatDetailsManager.GetResourceRecovery(splitCombatData, Units);
            if (!string.IsNullOrEmpty(playerId) && resourceRecovery != null && ResourcesRecoveries.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), resourceRecovery);
            }
        }
    }

    private void AddDamageDone(DamageDone damageDone, string[] playersId)
    {
        var selectedId = damageDone.Creator.GameId;
        if (!selectedId.Contains("Player"))
        {
            var playerCreature = Units
                .FirstOrDefault(x => x.Value.CreatorGameId != null && x.Value.GameId == damageDone.Creator.GameId && playersId.Contains(x.Value.CreatorGameId)).Value;
            if (playerCreature == null)
            {
                return;
            }

            damageDone.Spell = $"{playerCreature.Name} - {damageDone.Spell}";
            selectedId = playerCreature.CreatorGameId!;
        }

        if (DamageDones.TryGetValue(selectedId, out var collection))
        {
            collection.TryAdd(Guid.NewGuid().ToString(), damageDone);
        }
        else
        {
            var newDictionary = new ConcurrentDictionary<string, DamageDone>();
            newDictionary.TryAdd(Guid.NewGuid().ToString(), damageDone);
            DamageDones.TryAdd(selectedId, newDictionary);
        }
    }
}
