using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Extensions;

public static class CombatDetailsExtension
{
    public static void CalculateGeneralData(this CombatDetails combatDetails, List<string> playersId, string duration)
    {
        try
        {
            if (playersId == null || playersId.Count == 0)
            {
                throw new ArgumentNullException(nameof(playersId));
            }
            else if (string.IsNullOrEmpty(duration))
            {
                throw new ArgumentNullException(nameof(duration));
            }

            foreach (var playerId in playersId)
            {
                combatDetails.DamageDoneGeneral.TryAdd(playerId, GetDamageDoneGeneral(combatDetails.DamageDone[playerId], duration));
                combatDetails.HealDoneGeneral.TryAdd(playerId, GetHealDoneGeneral(combatDetails.HealDone[playerId], duration));
                combatDetails.DamageTakenGeneral.TryAdd(playerId, GetDamageTakenGeneral(combatDetails.DamageTaken[playerId], duration));
                combatDetails.ResourcesRecoveryGeneral.TryAdd(playerId, GetResourceRecoveryGeneral(combatDetails.ResourcesRecovery[playerId], duration));
            }
        }
        catch (ArgumentNullException ex)
        {
            combatDetails.Logger.LogError(ex, ex.Message, ex.ParamName);
        }
        catch (Exception ex)
        {
            combatDetails.Logger.LogError(ex, ex.Message);
        }
    }

    private static List<DamageDoneGeneral> GetDamageDoneGeneral(List<DamageDone> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList()).ToList();

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return null;
        }

        var lessDetails = new List<DamageDoneGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var damagePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var damagePerSecondRound = double.Round(damagePerSecond, 2);

            var damageDoneGeneral = new DamageDoneGeneral
            {
                Value = item.Sum(x => x.Value),
                DamagePerSecond = damagePerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                SpellOrItem = item[0].SpellOrItem,
                CastNumber = item.Count,
                IsPet = item.FirstOrDefault().IsPet
            };

            lessDetails.Add(damageDoneGeneral);
        }

        lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();

        return lessDetails;
    }

    private static List<HealDoneGeneral> GetHealDoneGeneral(List<HealDone> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return null;
        }

        var lessDetails = new List<HealDoneGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var healPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var healPerSecondRound = double.Round(healPerSecond, 2);

            var healDoneGeneral = new HealDoneGeneral
            {
                Value = item.Sum(x => x.Value),
                HealPerSecond = healPerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                SpellOrItem = item[0].SpellOrItem,
                CastNumber = item.Count,
                DamageAbsorbed = string.Empty
            };

            lessDetails.Add(healDoneGeneral);
        }

        lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();

        return lessDetails;
    }

    private static List<DamageTakenGeneral> GetDamageTakenGeneral(List<DamageTaken> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return null;
        }

        var lessDetails = new List<DamageTakenGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var damageTakenPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var damageTakenPerSecondRound = double.Round(damageTakenPerSecond, 2);

            var damageTakenGeneral = new DamageTakenGeneral
            {
                Value = item.Sum(x => x.Value),
                ActualValue = item.Sum(x => x.ActualValue),
                DamageTakenPerSecond = damageTakenPerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                SpellOrItem = item[0].SpellOrItem,
                CastNumber = item.Count,
            };

            lessDetails.Add(damageTakenGeneral);
        }

        lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();

        return lessDetails;
    }

    private static List<ResourceRecoveryGeneral> GetResourceRecoveryGeneral(List<ResourceRecovery> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return null;
        }

        var lessDetails = new List<ResourceRecoveryGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var resourcePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var resourcePerSecondRound = double.Round(resourcePerSecond, 2);

            var resourceRecoveryGeneral = new ResourceRecoveryGeneral
            {
                Value = item.Sum(x => x.Value),
                ResourcePerSecond = resourcePerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                SpellOrItem = item[0].SpellOrItem,
                CastNumber = item.Count,
            };

            lessDetails.Add(resourceRecoveryGeneral);
        }

        lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();

        return lessDetails;
    }
}
