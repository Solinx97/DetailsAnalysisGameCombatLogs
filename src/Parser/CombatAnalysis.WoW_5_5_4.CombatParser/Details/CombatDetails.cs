using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Details;

public class CombatDetails(ILogger logger)
{
    private readonly string[] _summon =
    [
        CombatLogKeyWords.SpellSummon,
    ];
    private readonly string[] _health =
    [
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
        CombatLogKeyWords.SpellAbsorbed,
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
    ];
    private readonly string[] _dieds =
    [
        CombatLogKeyWords.UnitDied,
    ];
    private readonly string[] _auras =
    [
        CombatLogKeyWords.AuraApplied,
        CombatLogKeyWords.AuraRemoved,
        CombatLogKeyWords.AuraAppliedDose,
        CombatLogKeyWords.AuraRemovedDose,
    ];
    private readonly string[] _casts =
    [
        CombatLogKeyWords.SpellCastStart,
        CombatLogKeyWords.SpellCastSuccess,
        CombatLogKeyWords.SpellCastFailed,
    ];
    private readonly string[] _positions =
    [
        CombatLogKeyWords.SpellCastSuccess,
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

    #region Details collections

    public ConcurrentDictionary<string, CombatUnit> Units { get; private set; } = [];

    public ConcurrentDictionary<string, List<UnitCast>> UnitCasts { get; private set; } = [];

    public ConcurrentDictionary<string, List<UnitHealth>> UnitHealths { get; private set; } = [];

    public ConcurrentDictionary<string, List<UnitPosition>> UnitPositions { get; private set; } = [];

    public ConcurrentDictionary<string, List<CombatPlayerAura>> Auras { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, CombatPlayerDeath>> Deathes { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageDone>> DamageDones { get; private set; } = [];

    public Dictionary<string, List<DamageDoneGeneral>> DamageDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, HealDone>> HealDones { get; private set; } = [];

    public Dictionary<string, List<HealDoneGeneral>> HealDoneGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, DamageTaken>> DamageTakens { get; private set; } = [];

    public Dictionary<string, List<DamageTakenGeneral>> DamageTakenGenerals { get; private set; } = [];

    public ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceRecovery>> ResourcesRecoveries { get; private set; } = [];

    public Dictionary<string, List<ResourceRecoveryGeneral>> ResourcesRecoveryGenerals { get; private set; } = [];

    #endregion

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

            foreach (var CombatDataLine in combatData)
            {
                Parse(playersId, CombatDataLine, combatStarted, combatFinished);
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

    private void PrepareCollections(string playersd)
    {
        UnitHealths.TryAdd(playersd, []);
        UnitPositions.TryAdd(playersd, []);
        Deathes.TryAdd(playersd, []);
        UnitCasts.TryAdd(playersd, []);
        Auras.TryAdd(playersd, []);

        DamageDones.TryAdd(playersd, []);
        HealDones.TryAdd(playersd, []);
        DamageTakens.TryAdd(playersd, []);
        ResourcesRecoveries.TryAdd(playersd, []);
    }

    private void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasSummon = _summon.Any(combatDataLine.Contains);
        var hasHealth = _health.Any(combatDataLine.Contains);
        var hasCasts = _casts.Any(combatDataLine.Contains);
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasSummon && !hasCasts && !hasPositions && !hasDieds 
            && !hasAuras && !hasHeal && !hasDamage && !hasAbsorb && !hasResources)
        {
            return;
        }

        var splitCombatData = SplitCombatData(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(playersId, combatStarted, combatFinished);

        if (hasSummon)
        {
            combatDetailsManager.GetSummonUnit(splitCombatData, Units);
        }

        Parallel.Invoke(
                () =>
                {
                    if (hasHealth || hasDieds)
                    {
                        CalculateHealth(combatDetailsManager, splitCombatData, hasDieds);
                    }
                },
                () =>
                {
                    if (hasCasts)
                    {
                        CalculateCasts(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    if (hasPositions)
                    {
                        CalculatePositions(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    if (hasDamage)
                    {
                        CalculateDamageTaken(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    CalculateGeneral(combatDataLine, combatDetailsManager, splitCombatData);
                }
            );
    }

    private void CalculateHealth(CombatDetailsManager combatDetailsManager, string[] splitCombatData, bool isDied)
    {
        var unitHealth = isDied 
            ? combatDetailsManager.GetUnitDeathHealth(splitCombatData, UnitHealths) 
            : combatDetailsManager.GetUnitHealth(splitCombatData, UnitHealths);
        if (unitHealth != null && UnitHealths.TryGetValue(unitHealth.CreatorGameId, out var collection))
        {
            collection.Add(unitHealth);
        }
    }

    private void CalculateCasts(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetCasts(splitCombatData, UnitCasts);
    }

    private void CalculatePositions(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetPosition(splitCombatData, UnitPositions, Units);
    }

    private void CalculateDamageTaken(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        var (playerId, damageTaken) = combatDetailsManager.GetDamageTaken(splitCombatData);
        if (!string.IsNullOrEmpty(playerId) && damageTaken != null && DamageTakens.TryGetValue(playerId, out var collection))
        {
            collection.TryAdd(Guid.NewGuid().ToString(), damageTaken);
        }
    }

    private void CalculateGeneral(string combatDataLine, CombatDetailsManager combatDetailsManager, string[] splitCombatData)
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
            var allPetsId = _petsId.SelectMany(x => x.Value).ToList();
            combatDetailsManager.GetAuras(splitCombatData, Auras, allPetsId);
        }
        else if (hasHeal)
        {
            var (playerId, healDone) = combatDetailsManager.GetHealDone(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) && healDone != null && HealDones.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), healDone);
            }
        }
        else if (hasAbsorb)
        {
            var (playerId, absorb) = combatDetailsManager.GetAbsorb(splitCombatData);
            if (absorb != null && HealDones.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), absorb);
            }
        }
        else if (hasDamage)
        {
            var (playerId, damageDone) = combatDetailsManager.GetPlayerDamageDone(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) && damageDone != null && DamageDones.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), damageDone);
            }

            (playerId, damageDone) = combatDetailsManager.GetPetsDamageDone(splitCombatData, _petsId);
            if (!string.IsNullOrEmpty(playerId) && damageDone != null && DamageDones.TryGetValue(playerId, out var colelction))
            {
                colelction.TryAdd(Guid.NewGuid().ToString(), damageDone);
            }
        }
        else if (hasResources)
        {
            var (playerId, resourceRecovery) = combatDetailsManager.GetResourceRecovery(splitCombatData);
            if (!string.IsNullOrEmpty(playerId) && resourceRecovery != null && ResourcesRecoveries.TryGetValue(playerId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), resourceRecovery);
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

        CheckComplexText(data);

        return [.. data];
    }

    private static void CheckComplexText(List<string> content)
    {
        var craft = string.Empty;
        var startIndex = -1;
        var finishIndex = -1;
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].StartsWith('\"') && !content[i].EndsWith('\"'))
            {
                craft += content[i];
                startIndex = i;
            }
            else if (!string.IsNullOrEmpty(craft) && !content[i].EndsWith('\"'))
            {
                craft += content[i];
            }
            else if (!string.IsNullOrEmpty(craft) && content[i].EndsWith('\"'))
            {
                craft += content[i];
                finishIndex = i;
                break;
            }
        }

        if (startIndex >= 0 && startIndex + 1 < content.Count && finishIndex >= 0)
        {
            content[startIndex] = craft;
            content.RemoveRange(startIndex + 1, finishIndex - startIndex);
        }
    }
}
