using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsDamageDone : CombatDetailsTemplate
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
        CombatLogKeyWords.SpellSummon,
    };
    private readonly ILogger _logger;

    public CombatDetailsDamageDone(ILogger logger) : base()
    {
        _logger = logger;
        DamageDone = new List<DamageDone>();
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        try
        {
            if (string.IsNullOrEmpty(playerId))
            {
                throw new ArgumentNullException(playerId);
            }

            var damageDone = GetSummaryDamageDone(playerId, combatData);

            return damageDone;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);

            return 0;
        }
    }

    private int GetSummaryDamageDone(string playerId, List<string> combatData)
    {
        int damageDone = 0;
        foreach (var item in combatData)
        {
            var itemHasDamageVariation = _damageVariations.Any(item.Contains);
            var succesfullCombatDataInformation = GetUsefulInformation(item);

            if (itemHasDamageVariation && item.Contains(playerId))
            {
                var damageDoneInformation = GetDamageDoneInformation(playerId, succesfullCombatDataInformation);
                if (damageDoneInformation == null)
                {
                    continue;
                }

                damageDone += damageDoneInformation.Value;
                DamageDone.Add(damageDoneInformation);
            }

            if (itemHasDamageVariation)
            {
                var damageDoneInformation = GetPetsDamageDoneInformation(playerId, succesfullCombatDataInformation);
                if (damageDoneInformation != null)
                {
                    damageDone += damageDoneInformation.Value;
                    DamageDone.Add(damageDoneInformation);
                }
            }
        }

        return damageDone;
    }

    private DamageDone GetDamageDoneInformation(string playerId, List<string> combatData)
    {
        if (string.Equals(combatData[1], CombatLogKeyWords.SpellSummon, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (!combatData[2].Contains(CombatLogKeyWords.Player))
        {
            return null;
        }
        else if (!combatData[2].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatData[^10], out var amountOfValue);

        string spellOrItem;
        if (string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem = CombatLogKeyWords.MeleeDamage;
        }
        else
        {
            spellOrItem = combatData[11].Trim('"');
        }

        var isPeriodicDamage = false;
        if (combatData[1] == CombatLogKeyWords.SpellPeriodicDamage)
        {
            isPeriodicDamage = true;
        }

        var isResist = false;
        var isImmune = false;
        var isParry = false;
        var isDodge = false;
        var isMiss = false;

        if (string.Equals(combatData[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        }
        else if (string.Equals(combatData[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isParry = string.Equals(combatData[13], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
            isDodge = string.Equals(combatData[13], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
            isMiss = string.Equals(combatData[13], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        }
        else if (string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[10], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isParry = string.Equals(combatData[10], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
            isDodge = string.Equals(combatData[10], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[10], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
            isMiss = string.Equals(combatData[10], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        }

        var isCrit = string.Equals(combatData[^3], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var damageDone = new DamageDone
        {
            Value = amountOfValue,
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

    private DamageDone GetPetsDamageDoneInformation(string playerId, List<string> combatData)
    {
        if (string.Equals(combatData[1], CombatLogKeyWords.SpellSummon, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (combatData[2].Contains(CombatLogKeyWords.Player))
        {
            return null;
        }
        else if (!combatData[2].Contains(CombatLogKeyWords.Creature) && !combatData[2].Contains(CombatLogKeyWords.Pet))
        {
            return null;
        }

        var currentPet = string.Empty;
        var petPlayer = string.Empty;
        foreach (var item in PetsId)
        {
            var pets = item.Value;

            currentPet = pets.Where(x => x.Equals(combatData[2])).FirstOrDefault();
            if (!string.IsNullOrEmpty(currentPet))
            {
                petPlayer = item.Key;
                break;
            }
        }

        if (string.IsNullOrEmpty(petPlayer) || petPlayer != playerId)
        {
            return null;
        }

        int.TryParse(combatData[^10], out var amountOfValue);

        var spellOrItem = $"{combatData[3].Trim('"')} - ";
        if (string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem += CombatLogKeyWords.MeleeDamage;
        }
        else
        {
            spellOrItem += combatData[11].Trim('"');
        }

        var isPeriodicDamage = false;
        if (combatData[1] == CombatLogKeyWords.SpellPeriodicDamage)
        {
            isPeriodicDamage = true;
        }

        var isResist = false;
        var isImmune = false;
        var isParry = false;
        var isDodge = false;
        var isMiss = false;

        if (string.Equals(combatData[1], CombatLogKeyWords.DamageShieldMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
        }
        else if (string.Equals(combatData[1], CombatLogKeyWords.SpellMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[13], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isParry = string.Equals(combatData[13], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
            isDodge = string.Equals(combatData[13], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[13], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
            isMiss = string.Equals(combatData[13], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        }
        else if (string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            isResist = string.Equals(combatData[10], CombatLogKeyWords.Resist, StringComparison.OrdinalIgnoreCase);
            isParry = string.Equals(combatData[10], CombatLogKeyWords.Parry, StringComparison.OrdinalIgnoreCase);
            isDodge = string.Equals(combatData[10], CombatLogKeyWords.Dodge, StringComparison.OrdinalIgnoreCase);
            isImmune = string.Equals(combatData[10], CombatLogKeyWords.Immune, StringComparison.OrdinalIgnoreCase);
            isMiss = string.Equals(combatData[10], CombatLogKeyWords.Miss, StringComparison.OrdinalIgnoreCase);
        }

        var isCrit = string.Equals(combatData[^3], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var damageDone = new DamageDone
        {
            Value = amountOfValue,
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
            IsPet = true,
        };

        return damageDone;
    }
}
