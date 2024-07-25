using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsHealDone : CombatDetailsTemplate
{
    private readonly string[] _healVariations = new string[]
    {
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
        CombatLogKeyWords.SpellAbsorbed,
    };
    private readonly string[] _absorbVariations = new string[]
    {
        CombatLogKeyWords.SpellAbsorbed,
    };
    private readonly ILogger _logger;

    public CombatDetailsHealDone(ILogger logger) : base()
    {
        _logger = logger;
        HealDone = new List<HealDone>();
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        try
        {
            if (playerId == null)
            {
                throw new ArgumentNullException(playerId);
            }

            var healthDone = GetSummaryHealDone(playerId, combatData);

            return healthDone;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);

            return 0;
        }
    }

    private int GetSummaryHealDone(string playerId, List<string> combatData)
    {
        int healthDone = 0;
        foreach (var item in combatData)
        {
            var itemHasHealVariation = _healVariations.Any(item.Contains);
            if (itemHasHealVariation && item.Contains(playerId))
            {
                var usefulInformation = GetUsefulInformation(item);
                var healDoneInformation = GetHealDoneInformation(playerId, usefulInformation);

                if (healDoneInformation == null)
                {
                    continue;
                }

                healthDone += healDoneInformation.Value;
                HealDone.Add(healDoneInformation);
            }

            var itemHasAbsrobVariation = _absorbVariations.Any(item.Contains);
            if (itemHasAbsrobVariation && item.Contains(playerId))
            {
                var usefulInformation = GetUsefulInformation(item);
                var absorbInformation = GetAbsorbDoneInformation(playerId, usefulInformation);

                if (absorbInformation != null)
                {
                    healthDone += absorbInformation.Value;
                    HealDone.Add(absorbInformation);
                }
            }
        }

        return healthDone;
    }

    private HealDone GetHealDoneInformation(string playerId, List<string> combatData)
    {
        if (!combatData[2].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatData[^4], out var value3);
        int.TryParse(combatData[^3], out var value4);

        var isCrit = string.Equals(combatData[^1], CombatLogKeyWords.IsCrit, StringComparison.OrdinalIgnoreCase);

        var healDone = new HealDone
        {
            Time = TimeSpan.Parse(combatData[0]),
            FromPlayer = combatData[3].Trim('"'),
            ToPlayer = combatData[7].Trim('"'),
            SpellOrItem = combatData[11].Trim('"'),
            DamageAbsorbed = string.Empty,
            ValueWithOverheal = value3,
            Overheal = value4,
            Value = value3 - value4,
            IsFullOverheal = value3 - value4 == 0,
            IsCrit = isCrit
        };

        return healDone;
    }

    private HealDone GetAbsorbDoneInformation(string playerId, List<string> combatData)
    {
        var countDataWithMeleeDamage = 19;

        if (!combatData[10].Equals(playerId) && !combatData[13].Equals(playerId))
        {
            return null;
        }

        int.TryParse(combatData[^2], out var amountOfHeal);

        var absorbeDone = new HealDone
        {
            Time = TimeSpan.Parse(combatData[0]),
            FromPlayer = combatData[^8].Trim('"'),
            ToPlayer = combatData[7].Trim('"'),
            SpellOrItem = combatData[^4].Trim('"'),
            DamageAbsorbed = combatData.Count > countDataWithMeleeDamage ? combatData[11].Trim('"') : CombatLogKeyWords.MeleeDamage,
            ValueWithOverheal = amountOfHeal,
            Overheal = 0,
            Value = amountOfHeal,
            IsFullOverheal = false,
            IsCrit = false,
            IsAbsorbed = true
        };

        return absorbeDone;
    }
}
