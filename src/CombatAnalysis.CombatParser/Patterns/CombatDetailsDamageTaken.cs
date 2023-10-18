using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsDamageTaken : CombatDetailsTemplate
{
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
    private readonly ILogger _logger;

    public CombatDetailsDamageTaken(ILogger logger) : base()
    {
        _logger = logger;
        DamageTaken = new List<DamageTaken>();
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        int damageTaken = 0;
        try
        {
            if (playerId == null)
            {
                throw new ArgumentNullException(playerId);
            }

            foreach (var item in combatData)
            {
                var itemHasDamageVariation = _damageVariations.Any(item.Contains);
                if (itemHasDamageVariation && item.Contains(playerId))
                {
                    var usefulInformation = GetUsefulInformation(item);
                    var damageTakenInformation = GetDamageTakenInformation(playerId, usefulInformation);

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
            _logger.LogError(ex, ex.Message, playerId);
        }

        return damageTaken;
    }

    private DamageTaken GetDamageTakenInformation(string playerId, List<string> combatData)
    {
        //if (!combatData[2].Equals(playerId))
        //{
        //    return null;
        //}

        if (string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (combatData[2].Contains("0000000000000000")
            || combatData[2].Contains(CombatLogKeyWords.Creature))
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
            ? CombatLogKeyWords.MeleeDamage : combatData[11].Trim('"');

        var isResist = false;
        var isImmune = false;
        var isAbsorb = false;

        var isCrushing = string.Equals(combatData[^1], CombatLogKeyWords.IsCrushing, StringComparison.OrdinalIgnoreCase);

        int realDamage = 0;
        int mitigated = 0;
        int absorb = 0;
        int blocked = 0;
        int resist = 0;

        if (string.Equals(combatData[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
            isAbsorb = string.Equals(combatData[13], CombatLogKeyWords.Absorb, StringComparison.OrdinalIgnoreCase);

            int.TryParse(combatData[^1], out realDamage);
            int.TryParse(combatData[^2], out absorb);
        }
        else if (!string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase) 
            && !string.Equals(combatData[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase) 
            && !string.Equals(combatData[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
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
            IsDodge = string.Equals(combatData[^2], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase),
            IsParry = string.Equals(combatData[^2], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase),
            IsMiss = string.Equals(combatData[^2], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase),
            IsResist = isResist,
            IsImmune = isImmune,
            IsAbsorb = isAbsorb,
            IsCrushing = isCrushing,
        };

        return damageTaken;
    }
}
