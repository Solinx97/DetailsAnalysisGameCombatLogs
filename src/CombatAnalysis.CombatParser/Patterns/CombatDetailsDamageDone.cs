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
    };
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
                var itemHasDamageVariation = _damageVariations.Any(item.Contains);
                if (itemHasDamageVariation && item.Contains(player))
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
            || string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        int.TryParse(combatData[^10], out var value1);
        string spellOrItem;

        if (string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem = "Ближ. бой";
        }
        else
        {
            spellOrItem = combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil")
                ? "Ближ. бой" : combatData[11].Trim('"');
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
