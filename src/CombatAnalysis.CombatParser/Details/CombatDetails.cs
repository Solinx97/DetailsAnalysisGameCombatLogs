using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Details;

public class CombatDetails
{
    private readonly string[] _positions = new string[]
    {
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SpellCastSuccess,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
    };
    private readonly string[] _healVariations = new string[]
    {
        CombatLogKeyWords.SpellHeal,
    };
    private readonly string[] _absorbVariations = new string[]
    {
        CombatLogKeyWords.SpellAbsorbed,
    };
    private readonly string[] _damageVariations = new string[]
    {
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
    };
    private readonly string[] _resourceVariations = new string[]
    {
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    };

    private readonly ILogger _logger;
    private readonly Dictionary<string, List<string>> _petsId;

    public Dictionary<string, List<CombatPlayerPosition>> Positions { get; private set; }

    public Dictionary<string, List<DamageDone>> DamageDone { get; private set; }

    public Dictionary<string, List<HealDone>> HealDone { get; private set; }

    public Dictionary<string, List<DamageTaken>> DamageTaken { get; private set; }

    public Dictionary<string, List<ResourceRecovery>> ResourcesRecovery { get; private set; }

    public CombatDetails(ILogger logger)
    {
        _logger = logger;
        Positions = new Dictionary<string, List<CombatPlayerPosition>>();
        DamageDone = new Dictionary<string, List<DamageDone>>();
        HealDone = new Dictionary<string, List<HealDone>>();
        DamageTaken = new Dictionary<string, List<DamageTaken>>();
        ResourcesRecovery = new Dictionary<string, List<ResourceRecovery>>();
    }

    public CombatDetails(ILogger logger, Dictionary<string, List<string>> petsId) : this(logger)
    {
        _petsId = petsId;
    }

    public void Calculate(List<string> playersId, List<string> combatData)
    {
        try
        {
            if (playersId == null || playersId.Count == 0)
            {
                throw new ArgumentNullException(nameof(playersId));
            }
            else if (combatData == null || combatData.Count == 0)
            {
                throw new ArgumentNullException(nameof(combatData));
            }

            PrepareCollections(playersId);

            foreach (var item in combatData)
            {
                Parse(playersId, item);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, ex.ParamName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void PrepareCollections(List<string> playersId)
    {
        foreach(var playerId in playersId)
        {
            Positions.Add(playerId, new List<CombatPlayerPosition>());
            DamageDone.Add(playerId, new List<DamageDone>());
            HealDone.Add(playerId, new List<HealDone>());
            DamageTaken.Add(playerId, new List<DamageTaken>());
            ResourcesRecovery.Add(playerId, new List<ResourceRecovery>());
        }
    }

    private void Parse(List<string> playersId, string combatDataLine)
    {
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasPositions && !hasHeal && !hasDamage && !hasAbsorb && !hasResources)
        {
            return;
        }

        var clearCombatData = RemoveTime(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(playersId);

        if (hasPositions)
        {
            var (playerId, positionsInformation) = combatDetailsManager.GetPositions(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || positionsInformation != null)
            {
                if (Positions.TryGetValue(playerId, out var collection))
                {
                    collection.Add(positionsInformation);
                }
            }
        }

        if (hasHeal)
        {
            var (playerId, healDoneInformation) = combatDetailsManager.GetHealDone(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || healDoneInformation != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(healDoneInformation);
                }
            }
        }
        else if (hasAbsorb)
        {
            var (playerId, absorbInformation) = combatDetailsManager.GetAbsorb(clearCombatData);
            if (absorbInformation != null)
            {
                if (HealDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(absorbInformation);
                }
            }
        }
        else if (hasDamage)
        {
            var (playerId, damageDoneInformation) = combatDetailsManager.GetPlayerDamageDone(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageDoneInformation != null)
            {
                if (DamageDone.TryGetValue(playerId, out var collection))
                {
                    collection.Add(damageDoneInformation);
                }
            }

            (playerId, damageDoneInformation) = combatDetailsManager.GetPetsDamageDone(clearCombatData, _petsId);
            if (!string.IsNullOrEmpty(playerId) || damageDoneInformation != null)
            {
                if (DamageDone.TryGetValue(playerId, out var colelction))
                {
                    colelction.Add(damageDoneInformation);
                }
            }
        }
        else if (hasResources)
        {
            var (playerId, energyRecoveryInformation) = combatDetailsManager.GetEnergyRecovery(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || energyRecoveryInformation != null)
            {
                if (ResourcesRecovery.TryGetValue(playerId, out var collection))
                {
                    collection.Add(energyRecoveryInformation);
                }
            }
        }

        if (hasDamage)
        {
            var (playerId, damageTakenInformation) = combatDetailsManager.GetDamageTaken(clearCombatData);
            if (!string.IsNullOrEmpty(playerId) || damageTakenInformation != null)
            {
                if (DamageTaken.TryGetValue(playerId, out var collection))
                {
                    collection.Add(damageTakenInformation);
                }
            }
        }
    }

    private static List<string> RemoveTime(string combatData)
    {
        var log = combatData.Split("  ");
        var parse = log[1].Split(',');
        var time = log[0].Split(' ');

        var data = new List<string>
        {
            time[1]
        };

        data.AddRange(parse);

        return data;
    }
}
