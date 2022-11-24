using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsDamageTaken : CombatDetailsTemplate
{
    private readonly string[] _damageVariations = new string[]
    {   
        CombatLogConsts.SpellDamage,
        CombatLogConsts.SwingDamage,
        CombatLogConsts.SpellPeriodicDamage,
        CombatLogConsts.SwingMissed,
        CombatLogConsts.DamageShieldMissed,
        CombatLogConsts.RangeDamage,
        CombatLogConsts.SpellMissed,
    };
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
                var itemHasDamageVariation = _damageVariations.Any(damagVariation => item.Contains(damagVariation));
                if (itemHasDamageVariation && item.Contains(player))
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
        if (string.Equals(combatData[1], CombatLogConsts.SwingDamageLanded, StringComparison.OrdinalIgnoreCase))
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

        var isCrushing = string.Equals(combatData[^1], "1", StringComparison.OrdinalIgnoreCase);

        int realDamage = 0;
        int mitigated = 0;
        int absorb = 0;
        int blocked = 0;
        int resist = 0;

        if (string.Equals(combatData[1], CombatLogConsts.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogConsts.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], "RESIST", StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], "IMMUNE", StringComparison.OrdinalIgnoreCase);
            isAbsorb = string.Equals(combatData[13], "ABSORB", StringComparison.OrdinalIgnoreCase);

            int.TryParse(combatData[^1], out realDamage);
            int.TryParse(combatData[^2], out absorb);
        }
        else if (!string.Equals(combatData[1], CombatLogConsts.SwingMissed, StringComparison.OrdinalIgnoreCase) 
            && !string.Equals(combatData[1], CombatLogConsts.SpellMissed, StringComparison.OrdinalIgnoreCase) 
            && !string.Equals(combatData[1], CombatLogConsts.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
        {
            int.TryParse(combatData[^9], out realDamage);
            int.TryParse(combatData[^4], out absorb);
            int.TryParse(combatData[^5], out blocked);
            int.TryParse(combatData[^6], out resist);

            mitigated = realDamage - value;
        }

        var isPeriodicDamage = false;
        var enemy = combatData[3];
        if (string.Equals(combatData[3], "nil", StringComparison.OrdinalIgnoreCase))
        {
            isPeriodicDamage = true;
            enemy = combatData[11];
        }

        var damageTaken = new DamageTaken
        {
            Value = value,
            Time = TimeSpan.Parse(combatData[0]),
            FromEnemy = enemy.Trim('"'),
            ToPlayer = combatData[7].Trim('"'),
            SpellOrItem = spellOrItem,
            IsPeriodicDamage = isPeriodicDamage,
            Resisted = resist,
            Absorbed = absorb,
            Blocked = blocked,
            RealDamage = realDamage,
            Mitigated = mitigated < 0 ? 0 : mitigated,
            IsDodge = string.Equals(combatData[^2], "DODGE", StringComparison.OrdinalIgnoreCase),
            IsParry = string.Equals(combatData[^2], "PARRY", StringComparison.OrdinalIgnoreCase),
            IsMiss = string.Equals(combatData[^2], "MISS", StringComparison.OrdinalIgnoreCase),
            IsResist = isResist,
            IsImmune = isImmune,
            IsAbsorb = isAbsorb,
            IsCrushing = isCrushing,
        };

        return damageTaken;
    }
}
