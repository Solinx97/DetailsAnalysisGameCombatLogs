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
                        var damageTakenInformation = GetDamageTakenInformation(usefulInformation);

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

        private DamageTaken GetDamageTakenInformation(List<string> combatData)
        {
            if (combatData[1] == "SWING_DAMAGE_LANDED")
            {
                return null;
            }

            if (combatData[2].Contains("0000000000000000")
                || combatData[2].Contains("Creature"))
            {

                var damageTaken = GetDamageTaken(combatData);
                return damageTaken;
            }
            else
            {
                return null;
            }
        }

        private DamageTaken GetDamageTaken(List<string> combatData)
        {
            int.TryParse(combatData[^10], out var value);

            var spellOrItem = combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil")
                ? "Ближ. бой" : combatData[11].Trim('"');

            var isResist = false;
            var isImmune = false;
            var isAbsorb = false;

            var isCrushing = combatData[^1] == "1" ? true : false;

            int realDamage = 0;
            int mitigated = 0;
            int absorb = 0;
            int blocked = 0;
            int resist = 0;

            if (combatData[1] == "DAMAGE_SHIELD_MISSED"
                || combatData[1] == "SPELL_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
                isAbsorb = combatData[13] == "ABSORB" ? true : false;

                int.TryParse(combatData[^1], out realDamage);
                int.TryParse(combatData[^2], out absorb);
            }
            else if (combatData[1] != "SWING_MISSED"
                && combatData[1] != "SPELL_MISSED"
                && combatData[1] != "DAMAGE_SHIELD_MISSED")
            {
                int.TryParse(combatData[^9], out realDamage);
                int.TryParse(combatData[^4], out absorb);
                int.TryParse(combatData[^5], out blocked);
                int.TryParse(combatData[^6], out resist);

                mitigated = realDamage - value;
            }

            var damageTaken = new DamageTaken
            {
                Value = value,
                Time = TimeSpan.Parse(combatData[0]),
                FromEnemy = combatData[3].Trim('"'),
                ToPlayer = combatData[7].Trim('"'),
                SpellOrItem = spellOrItem,
                Resist = resist,
                Absorb = absorb,
                Blocked = blocked,
                RealDamage = realDamage,
                Mitigated = mitigated,
                IsDodge = combatData[^2] == "DODGE",
                IsParry = combatData[^2] == "PARRY",
                IsMiss = combatData[^2] == "MISS",
                IsResist = isResist,
                IsImmune = isImmune,
                IsAbsorb = isAbsorb,
                IsCrushing = isCrushing,
            };

            return damageTaken;
        }
    }
}
