﻿using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Patterns
{
    public class CombatDetailsHealDone : CombatDetailsTemplate
    {
        private readonly ILogger _logger;

        public CombatDetailsHealDone(ILogger logger) : base()
        {
            _logger = logger;
            HealDone = new List<HealDone>();
        }

        public override int GetData(string player, List<string> combatData)
        {
            int healthDone = 0;
            try
            {
                if (player == null)
                {
                    throw new ArgumentNullException(player);
                }

                foreach (var item in combatData)
                {
                    if ((item.Contains("SPELL_HEAL") || item.Contains("SPELL_PERIODIC_HEAL"))
                        && item.Contains(player))
                    {
                        var usefulInformation = GetUsefulInformation(item);
                        var healDoneInformation = GetHealDoneInformation(player, usefulInformation);

                        if (healDoneInformation != null)
                        {
                            healthDone += healDoneInformation.Value;
                            HealDone.Add(healDoneInformation);
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message, player);
            }

            return healthDone;
        }

        private HealDone GetHealDoneInformation(string player, List<string> combatData)
        {
            if (!combatData[3].Contains(player))
            {
                return null;
            }

            int.TryParse(combatData[21], out var value1);
            int.TryParse(combatData[^1], out var value2);
            int.TryParse(combatData[^4], out var value3);
            int.TryParse(combatData[^3], out var value4);

            var isCrit = combatData[^1] == "1" ? true : false;

            var healDone = new HealDone
            {
                CurrentHealth = value1,
                Time = TimeSpan.Parse(combatData[0]),
                FromPlayer = combatData[3].Trim('"'),
                ToPlayer = combatData[7].Trim('"'),
                SpellOrItem = combatData[11].Trim('"'),
                MaxHealth = value2,
                ValueWithOverheal = value3,
                Overheal = value4,
                Value = value3 - value4,
                IsFullOverheal = value3 - value4 == 0,
                IsCrit = isCrit
            };

            return healDone;
        }
    }
}