using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Patterns
{
    public class CombatDetailsDamageDone : CombatDetailsTemplate
    {
        private readonly ILogger _logger;

        public CombatDetailsDamageDone(ILogger logger) : base()
        {
            _logger = logger;
            DamageDone = new List<DamageDone>();
        }

        public override int GetData(string player, List<string> combatData)
        {
            int damageDone = 0;
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
                        var succesfullCombatDataInformation = GetUsefulInformation(item);
                        var damageDoneInformation = GetDamageDoneInformation(player, succesfullCombatDataInformation);

                        if (damageDoneInformation != null)
                        {
                            damageDone += damageDoneInformation.Value;
                            DamageDone.Add(damageDoneInformation);
                        }
                    }
                }

                return damageDone;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message, player);

                return damageDone;
            }
        }

        private DamageDone GetDamageDoneInformation(string player, List<string> combatData)
        {
            if (!combatData[3].Contains(player)
                || combatData[1] == "SWING_DAMAGE_LANDED")
            {
                return null;
            }

            int.TryParse(combatData[^10], out var value1);
            string spellOrItem;

            if (combatData[1] == "SWING_MISSED")
            {
                spellOrItem = "Ближ. бой";
            }
            else
            {
                spellOrItem = combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil")
                    ? "Ближ. бой" : combatData[11].Trim('"');
            }

            var isPeriodicDamage = false;
            if (combatData[1] == "SPELL_PERIODIC_DAMAGE")
            {
                isPeriodicDamage = true;
            }

            var isResist = false;
            var isImmune = false;
            var isParry = false;
            var isDodge = false;
            var isMiss = false;
            if (combatData[1] == "DAMAGE_SHIELD_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
            }
            else if (combatData[1] == "SPELL_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isParry = combatData[13] == "PARRY" ? true : false;
                isDodge = combatData[13] == "DODGE" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
                isMiss = combatData[13] == "MISS" ? true : false;
            }
            else if (combatData[1] == "SWING_MISSED")
            {
                isResist = combatData[10] == "RESIST" ? true : false;
                isParry = combatData[10] == "PARRY" ? true : false;
                isDodge = combatData[10] == "DODGE" ? true : false;
                isImmune = combatData[10] == "IMMUNE" ? true : false;
                isMiss = combatData[10] == "MISS" ? true : false;
            }

            var isCrit = combatData[^3] == "1" ? true : false;

            var damageDone = new DamageDone
            {
                Value = value1,
                Time = TimeSpan.Parse(combatData[0]),
                FromPlayer = combatData[3].Trim('"'),
                ToEnemy = combatData[7].Trim('"'),
                SpellOrItem = spellOrItem,
                IsPeriodicDamage = isPeriodicDamage,
                IsDodge = isDodge,
                IsMiss = isMiss,
                IsParry = isParry,
                IsResist = isResist,
                IsImmune = isImmune,
                IsCrit = isCrit,
            };

            return damageDone;
        }
    }
}
