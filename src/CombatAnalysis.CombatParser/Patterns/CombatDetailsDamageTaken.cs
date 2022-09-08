using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Patterns
{
    public class CombatDetailsDamageTaken : CombatDetailsTemplate
    {
        private readonly ILogger _logger;

        public CombatDetailsDamageTaken(ILogger logger) : base()
        {
            _logger = logger;
            DamageTaken = new List<DamageTaken>();
        }

        public override int GetData(string player, List<string> combatData)
        {
            int damageTaken = 0;
            try
            {
                if (player == null)
                {
                    throw new ArgumentNullException(player);
                }

                foreach (var item in combatData)
                {
                    if ((item.Contains("SPELL_DAMAGE") || item.Contains("SWING_DAMAGE")
                        || item.Contains("SPELL_PERIODIC_DAMAGE") || item.Contains("SWING_MISSED")
                        || item.Contains("DAMAGE_SHIELD_MISSED") || item.Contains("RANGE_DAMAGE")
                        || item.Contains("SPELL_MISSED")) && item.Contains(player))
                    {
                        var usefulInformation = GetUsefulInformation(item);
                        var damageTakenInformation = GetDamageTakenInformation(player, usefulInformation);

                        if (damageTakenInformation != null)
                        {
                            damageTaken += damageTakenInformation.Value;
                            DamageTaken.Add(damageTakenInformation);
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message, player);
            }

            return damageTaken;
        }

        private DamageTaken GetDamageTakenInformation(string player, List<string> combatData)
        {
            if (combatData[1] == "SWING_DAMAGE_LANDED")
            {
                return null;
            }

            if (combatData[2].Contains("0000000000000000")
                || combatData[2].Contains("Creature"))
            {
                int.TryParse(combatData[^10], out var value1);
                var spellOrItem = combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil")
                    ? "Ближ. бой" : combatData[11].Trim('"');

                var isResist = false;
                var isImmune = false;
                if (combatData[1] == "DAMAGE_SHIELD_MISSED")
                {
                    isResist = combatData[13] == "RESIST" ? true : false;
                    isImmune = combatData[13] == "IMMUNE" ? true : false;
                }

                var isCrushing = combatData[^1] == "1" ? true : false;

                var damageTaken = new DamageTaken
                {
                    Value = value1,
                    Time = TimeSpan.Parse(combatData[0]),
                    From = combatData[3].Trim('"'),
                    To = combatData[7].Trim('"'),
                    SpellOrItem = spellOrItem,
                    IsDodge = combatData[10] == "DODGE",
                    IsParry = combatData[10] == "PARRY",
                    IsMiss = combatData[10] == "MISS",
                    IsResist = isResist,
                    IsImmune = isImmune,
                    IsCrushing = isCrushing,
                };

                return damageTaken;
            }
            else
            {
                return null;
            }
        }
    }
}
