using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.CombatParser.Details;

public class CombatDetails(ILogger logger)
{
    private readonly string[] _dieds =
    [
        CombatLogKeyWords.UnitDied,
    ];
    private readonly string[] _auras =
    [
        CombatLogKeyWords.AuraApplied,
        CombatLogKeyWords.AuraAppliedDose,
        CombatLogKeyWords.AuraRemoved,
    ];
    private readonly string[] _positions =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.SpellCastSuccess,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
    ];
    private readonly string[] _healVariations =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
    ];
    private readonly string[] _absorbVariations =
    [
        CombatLogKeyWords.SpellAbsorbed,
    ];
    private readonly string[] _damageVariations =
    [
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamageLanded,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
    ];
    private readonly string[] _resourceVariations =
    [
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    ];

    private readonly Dictionary<string, List<string>> _petsId = [];

    public ILogger Logger { get; private set; } = logger;

    public ConcurrentDictionary<string, ConcurrentDictionary<string, CombatPlayerPosition>> CombatPlayerPositions { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, CombatPlayerDeath>> CombatPlayersDeath { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageDone>> DamageDone { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageDoneGeneral { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, HealDone>> HealDone { get; private set; } = [];

    public Dictionary<string, List<HealDoneGeneral>> HealDoneGeneral { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageTaken>> DamageTaken { get; private set; } = [];

    public Dictionary<string, List<DamageTakenGeneral>> DamageTakenGeneral { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceRecovery>> ResourcesRecovery { get; private set; } = [];

    public Dictionary<string, List<ResourceRecoveryGeneral>> ResourcesRecoveryGeneral { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, CombatAura>> Auras { get; private set; } = [];

    public CombatDetails(ILogger logger, Dictionary<string, List<string>> petsId) : this(logger)
    {
        _petsId = petsId;
    }

    public void Calculate(string[] playersId, string[] combatData, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
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

            Parallel.ForEach(
                    combatData,
                    new ParallelOptions
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    },
                    combat => Parse(playersId, combat, combatStarted, combatFinished));
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

    private void PrepareCollections(string playersd)
    {
        CombatPlayerPositions.TryAdd(playersd, []);
        CombatPlayersDeath.TryAdd(playersd, []);
        Auras.TryAdd(playersd, []);

        DamageDone.TryAdd(playersd, []);
        HealDone.TryAdd(playersd, []);
        DamageTaken.TryAdd(playersd, []);
        ResourcesRecovery.TryAdd(playersd, []);
    }

    private void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasPositions && !hasDieds && !hasAuras && !hasHeal 
            && !hasDamage && !hasAbsorb && !hasResources)
        {
            return;
        }

        var splitCombatData = SplitCombatData(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(playersId, combatStarted, combatFinished);

        if (hasPositions)
        {
            var (playerId, positions) = combatDetailsManager.GetPositions(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || positions != null)
            {
                if (CombatPlayerPositions.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), positions);
                }
            }
        }

        if (hasDamage)
        {
            var (playerId, damageTaken) = combatDetailsManager.GetDamageTaken(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageTaken != null)
            {
                if (DamageTaken.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), damageTaken);
                }
            }
        }

        if (hasDieds)
        {
            var (playerId, playerDeath) = combatDetailsManager.GetPlayerDeath(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || playerDeath != null)
            {
                if (CombatPlayersDeath.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), playerDeath);
                }
            }
        }
        else if (hasAuras)
        {
            var allPetsId = _petsId.SelectMany(x => x.Value).ToList();
            var t = splitCombatData;
            var (creatorId, auras) = combatDetailsManager.GetAuras(t, Auras, allPetsId);
            if (!string.IsNullOrEmpty(creatorId) || auras != null)
            {
                if (Auras.TryGetValue(creatorId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), auras);
                }
                else
                {
                    var concurrentAuraColelction = new ConcurrentDictionary<string, CombatAura>();
                    concurrentAuraColelction.TryAdd(auras.Name, auras);
                    Auras.TryAdd(creatorId, concurrentAuraColelction);
                }
            }
        }
        else if (hasHeal)
        {
            var (playerId, healDone) = combatDetailsManager.GetHealDone(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || healDone != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), healDone);
                }
            }
        }
        else if (hasAbsorb)
        {
            var (playerId, absorb) = combatDetailsManager.GetAbsorb(splitCombatData);
            if (absorb != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), absorb);
                }
            }
        }
        else if (hasDamage)
        {
            var (playerId, damageDone) = combatDetailsManager.GetPlayerDamageDone(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageDone != null)
            {
                if (DamageDone.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), damageDone);
                }
            }

            (playerId, damageDone) = combatDetailsManager.GetPetsDamageDone(splitCombatData, _petsId);
            if (!string.IsNullOrEmpty(playerId) || damageDone != null)
            {
                if (DamageDone.TryGetValue(playerId, out var colelction))
                {
                    colelction.TryAdd(Guid.NewGuid().ToString(), damageDone);
                }
            }
        }
        else if (hasResources)
        {
            var (playerId, resourceRecovery) = combatDetailsManager.GetResourceRecovery(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) || resourceRecovery != null)
            {
                if (ResourcesRecovery.TryGetValue(playerId, out var collection))
                {
                    collection.TryAdd(Guid.NewGuid().ToString(), resourceRecovery);
                }
            }
        }
    }

    private static string[] SplitCombatData(string combatData)
    {
        var log = combatData.Split("  ");
        var parse = log[1].Split(',');

        var data = new List<string>
        {
            log[0],
        };

        data.AddRange(parse);

        return [.. data];
    }
}
