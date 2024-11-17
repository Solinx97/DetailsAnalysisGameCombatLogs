using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsResourceRecovery : BaseCombatDetails
{
    private readonly string[] _resourceVariations = new string[]
    {
        CombatLogKeyWords.SpellPeriodicEnergize,
        CombatLogKeyWords.SpellEnergize,
    };
    private readonly ILogger _logger;

    public CombatDetailsResourceRecovery(ILogger logger) : base()
    {
        _logger = logger;
        ResourceRecovery = new List<ResourceRecovery>();
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        try
        {
            if (playerId == null)
            {
                throw new ArgumentNullException(playerId);
            }

            var energyRecovery = GetSummaryEnergyRecovery(playerId, combatData);

            return energyRecovery;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);

            return 0;
        }
    }

    private int GetSummaryEnergyRecovery(string playerId, List<string> combatData)
    {
        int energyRecovery = 0;
        foreach (var item in combatData)
        {
            var itemHasResourceVariation = _resourceVariations.Any(item.Contains);
            if (itemHasResourceVariation && item.Contains(playerId))
            {
                var usefulInformation = GetUsefulInformation(item);
                var energyRecoveryInformation = GetEnergyInformation(playerId, usefulInformation);
                if (energyRecoveryInformation == null)
                {
                    continue;
                }

                energyRecovery += energyRecoveryInformation.Value;

                ResourceRecovery.Add(energyRecoveryInformation);
            }
        }

        return energyRecovery;
    }

    private ResourceRecovery GetEnergyInformation(string playerId, List<string> combatData)
    {
        if (!combatData[6].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatData[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var amoutOfResourcesRecovery);

        var spellOrItem = combatData[1].Contains(CombatLogKeyWords.SpellEnergize) ? combatData[11] : combatData[3];

        var energyRecovery = new ResourceRecovery
        {
            Time = TimeSpan.Parse(combatData[0]),
            Value = amoutOfResourcesRecovery,
            SpellOrItem = combatData[11].Trim('"')
        };

        return energyRecovery;
    }
}
