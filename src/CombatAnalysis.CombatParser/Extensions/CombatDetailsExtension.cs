using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Patterns;
using System.Collections.ObjectModel;

namespace CombatAnalysis.CombatParser.Extensions;

public static class CombatDetailsExtension
{
    public static ObservableCollection<DamageDoneGeneral> GetDamageDoneGeneral(this BaseCombatDetails extension, List<DamageDone> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList()).ToList();

        TimeSpan.TryParse(combat.Duration, out var durationTime);

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
        var damageDoneGroupBySpellOrItem = new ObservableCollection<DamageDoneGeneral>(lessDetails);

        return damageDoneGroupBySpellOrItem;
    }

    public static ObservableCollection<HealDoneGeneral> GetHealDoneGeneral(this BaseCombatDetails extension, List<HealDone> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

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
        var healDoneGroupBySpellOrItem = new ObservableCollection<HealDoneGeneral>(lessDetails);

        return healDoneGroupBySpellOrItem;
    }

    public static ObservableCollection<DamageTakenGeneral> GetDamageTakenGeneral(this BaseCombatDetails extension, List<DamageTaken> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

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
        var damageTakenGroupBySpellOrItem = new ObservableCollection<DamageTakenGeneral>(lessDetails);

        return damageTakenGroupBySpellOrItem;
    }

    public static ObservableCollection<ResourceRecoveryGeneral> GetResourceRecoveryGeneral(this BaseCombatDetails extension, List<ResourceRecovery> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

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
        var resourceRecoveryGroupBySpellOrItem = new ObservableCollection<ResourceRecoveryGeneral>(lessDetails);

        return resourceRecoveryGroupBySpellOrItem;
    }
}
