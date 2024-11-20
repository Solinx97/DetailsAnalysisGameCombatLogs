using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Details;

public class CombatDetails
{
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
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SwingDamageLanded,
    };
    private readonly string[] _resourceVariations = new string[]
    {
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    };
    private readonly ILogger _logger;
    private readonly Dictionary<string, List<string>> _petsId;

    public List<CombatPlayerPosition> Positions { get; private set; }

    public List<DamageDone> DamageDone { get; private set; }

    public List<HealDone> HealDone { get; private set; }

    public List<DamageTaken> DamageTaken { get; private set; }

    public List<ResourceRecovery> ResourcesRecovery { get; private set; }

    public CombatDetails(ILogger logger)
    {
        _logger = logger;
        Positions = new List<CombatPlayerPosition>();
        DamageDone = new List<DamageDone>();
        HealDone = new List<HealDone>();
        DamageTaken = new List<DamageTaken>();
        ResourcesRecovery = new List<ResourceRecovery>();
    }

    public CombatDetails(ILogger logger, Dictionary<string, List<string>> petsId) : this(logger)
    {
        _petsId = petsId;
    }

    public void Calculate(string playerId, List<string> combatData)
    {
        try
        {
            if (string.IsNullOrEmpty(playerId))
            {
                throw new ArgumentNullException(nameof(playerId));
            }
            else if (combatData == null || combatData.Count == 0)
            {
                throw new ArgumentNullException(nameof(combatData));
            }

            foreach (var item in combatData)
            {
                if (!item.Contains(playerId))
                {
                    continue;
                }

                var itemHasHealVariation = _healVariations.Any(item.Contains);
                var itemHasDamageVariation = _damageVariations.Any(item.Contains);
                var itemHasAbsorbVariation = _absorbVariations.Any(item.Contains);
                var itemHasResourceVariation = _resourceVariations.Any(item.Contains);

                var clearCombatData = RemoveTime(item);

                if ((itemHasHealVariation || itemHasDamageVariation))
                {
                    var positions = CombatDetailsManager.GetPositions(playerId, clearCombatData);
                    if (positions != null)
                    {
                        Positions.Add(positions);
                    }
                }

                if (itemHasHealVariation)
                {
                    var healDoneInformation = CombatDetailsManager.GetHealDone(playerId, clearCombatData);
                    if (healDoneInformation != null)
                    {
                        HealDone.Add(healDoneInformation);
                    }
                }
                else if (itemHasAbsorbVariation)
                {
                    var absorbInformation = CombatDetailsManager.GetAbsorb(playerId, clearCombatData);
                    if (absorbInformation != null)
                    {
                        HealDone.Add(absorbInformation);
                    }
                }
                else if (itemHasDamageVariation)
                {
                    var damageDoneInformation = CombatDetailsManager.GetPlayerDamageDone(playerId, clearCombatData);
                    if (damageDoneInformation != null)
                    {
                        DamageDone.Add(damageDoneInformation);
                    }
                }
                else if (itemHasDamageVariation)
                {
                    var damageDoneInformation = CombatDetailsManager.GetPetsDamageDone(playerId, clearCombatData, _petsId);
                    if (damageDoneInformation != null)
                    {
                        DamageDone.Add(damageDoneInformation);
                    }
                }
                else if (itemHasDamageVariation)
                {
                    var damageTakenInformation = CombatDetailsManager.GetDamageTaken(clearCombatData);
                    if (damageTakenInformation != null)
                    {
                        DamageTaken.Add(damageTakenInformation);
                    }
                }
                else if (itemHasResourceVariation)
                {
                    var energyRecoveryInformation = CombatDetailsManager.GetEnergyRecovery(playerId, clearCombatData);
                    if (energyRecoveryInformation != null)
                    {
                        ResourcesRecovery.Add(energyRecoveryInformation);
                    }
                }
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
