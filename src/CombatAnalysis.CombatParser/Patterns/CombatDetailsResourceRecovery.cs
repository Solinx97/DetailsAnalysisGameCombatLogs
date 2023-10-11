using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsResourceRecovery : CombatDetailsTemplate
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
        int energyRecovery = 0;
        try
        {
            if (playerId == null)
            {
                throw new ArgumentNullException(playerId);
            }

            foreach (var item in combatData)
            {
                var itemHasResourceVariation = _resourceVariations.Any(resourceVariation => item.Contains(resourceVariation));
                if (itemHasResourceVariation && item.Contains(playerId))
                {
                    var usefulInformation = GetUsefulInformation(item);
                    var energyRecoveryInformation = GetEnergyInformation(usefulInformation);
                    energyRecovery += energyRecoveryInformation.Value;

                    ResourceRecovery.Add(energyRecoveryInformation);
                }
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);
        }

        return energyRecovery;
    }

    private ResourceRecovery GetEnergyInformation(List<string> combatData)
    {
        int.TryParse(combatData[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var value4);

        var spellOrItem = combatData[1].Contains(CombatLogKeyWords.SpellEnergize) ? combatData[11] : combatData[3];

        var energyRecovery = new ResourceRecovery
        {
            Time = TimeSpan.Parse(combatData[0]),
            Value = value4,
            SpellOrItem = spellOrItem.Trim('"')
        };

        return energyRecovery;
    }
}
