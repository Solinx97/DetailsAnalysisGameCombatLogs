﻿using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Patterns
{
    public class CombatDetailsResourceRecovery : CombatDetailsTemplate
    {
        private readonly ILogger _logger;

        public CombatDetailsResourceRecovery(ILogger logger) : base()
        {
            _logger = logger;
            ResourceRecovery = new List<ResourceRecovery>();
        }

        public override int GetData(string player, List<string> combatData)
        {
            int energyRecovery = 0;
            try
            {
                if (player == null)
                {
                    throw new ArgumentNullException(player);
                }

                foreach (var item in combatData)
                {
                    if ((item.Contains("SPELL_PERIODIC_ENERGIZE") || item.Contains("SPELL_ENERGIZE"))
                        && item.Contains(player))
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
                _logger.LogError(ex, ex.Message, player);
            }

            return energyRecovery;
        }

        private ResourceRecovery GetEnergyInformation(List<string> combatData)
        {
            int.TryParse(combatData[^4], NumberStyles.Number, CultureInfo.InvariantCulture, out var value4);

            var spellOrItem = combatData[1].Contains("SPELL_ENERGIZE") ? combatData[11] : combatData[3];

            var energyRecovery = new ResourceRecovery
            {
                Time = TimeSpan.Parse(combatData[0]),
                Value = value4,
                SpellOrItem = spellOrItem.Trim('"')
            };

            return energyRecovery;
        }
    }
}