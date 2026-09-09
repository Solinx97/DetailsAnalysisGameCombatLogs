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
        CombatLogKeyWords.SwingDamage + ',',
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

    private void PrepareCollections(string playerId)
    {
        UnitPositions.TryAdd(playerId, []);
        Deathes.TryAdd(playerId, []);
        UnitCasts.TryAdd(playerId, []);
        Auras.TryAdd(playerId, []);

        HealDones.TryAdd(playerId, []);
        ResourcesRecoveries.TryAdd(playerId, []);
    }

    private void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasSummon = _summon.Any(combatDataLine.Contains);
        var hasCasts = _casts.Any(combatDataLine.Contains);
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasSummon && !hasCasts && !hasPositions && !hasDieds 
            && !hasAuras && !hasDamage && !hasAbsorb && !hasResources)
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
                    CalculateGeneral(combatDataLine, combatDetailsManager, splitCombatData, playersId);
                }
            );
    }

    private void CalculateCasts(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetCasts(splitCombatData, UnitCasts);
    }

    private void CalculatePositions(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        combatDetailsManager.GetPosition(splitCombatData, UnitPositions);
    }

    private void CalculateDamageTaken(CombatDetailsManager combatDetailsManager, string[] splitCombatData)
    {
        var (gameId, damageTaken) = combatDetailsManager.GetDamageDone(splitCombatData, false);
        if (!string.IsNullOrEmpty(gameId) && damageTaken != null && gameId.Contains("Player"))
        {
            if (DamageTakens.TryGetValue(damageTaken.TargetGameId, out var collection))
            {
                collection.TryAdd(Guid.NewGuid().ToString(), damageTaken);
            }
            else
            {
                var newDictionary = new ConcurrentDictionary<string, DamageDone>();
                newDictionary.TryAdd(Guid.NewGuid().ToString(), damageTaken);
                DamageTakens.TryAdd(damageTaken.TargetGameId, newDictionary);
            }
        }
    }

    private void CalculateGeneral(string combatDataLine, CombatDetailsManager combatDetailsManager, string[] splitCombatData, string[] playersId)
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
            var (gameId, damageDone) = combatDetailsManager.GetDamageDone(splitCombatData);
            if (!string.IsNullOrEmpty(gameId) && damageDone != null)
            {
                AddDamageDone(damageDone, playersId);
            }

            combatDetailsManager.GetCombatCreature(splitCombatData, Units);
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

    private void AddDamageDone(DamageDone damageDone, string[] playersId)
    {
        var selectedId = damageDone.CreatorGameId;
        if (!selectedId.Contains("Player"))
        {
            var playerCreature = Units
                .FirstOrDefault(x => x.Value.CreatorGameId != null && x.Value.GameId == damageDone.CreatorGameId && playersId.Contains(x.Value.CreatorGameId)).Value;
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
