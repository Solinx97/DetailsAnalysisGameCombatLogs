using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Patterns;
using System.Collections.ObjectModel;

namespace CombatAnalysis.CombatParser.Extensions;

public static class CombatDetailsExtension
{
    public static ObservableCollection<DamageDoneGeneral> GetDamageDoneGeneral(this CombatDetailsTemplate extension, List<DamageDone> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList()).ToList();

        TimeSpan.TryParse(combat.Duration, out var durationTime);

        var lessDetails = new List<DamageDoneGeneral>();
        foreach (var item in spells)
        {
            var damageDoneGeneral = new DamageDoneGeneral
            {
                Value = item.Sum(x => x.Value),
                DamagePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                AverageValue = item.Average(x => x.Value),
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

    public static ObservableCollection<HealDoneGeneral> GetHealDoneGeneral(this CombatDetailsTemplate extension, List<HealDone> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

        var lessDetails = new List<HealDoneGeneral>();
        foreach (var item in spells)
        {
            var healDoneGeneral = new HealDoneGeneral
            {
                Value = item.Sum(x => x.Value),
                HealPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                AverageValue = item.Average(x => x.Value),
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

    public static ObservableCollection<DamageTakenGeneral> GetDamageTakenGeneral(this CombatDetailsTemplate extension, List<DamageTaken> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

        var lessDetails = new List<DamageTakenGeneral>();
        foreach (var item in spells)
        {
            var damageTakenGeneral = new DamageTakenGeneral
            {
                Value = item.Sum(x => x.Value),
                ActualValue = item.Sum(x => x.ActualValue),
                DamageTakenPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                AverageValue = item.Average(x => x.Value),
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

    public static ObservableCollection<ResourceRecoveryGeneral> GetResourceRecoveryGeneral(this CombatDetailsTemplate extension, List<ResourceRecovery> collection, Combat combat)
    {
        var spells = collection
            .GroupBy(group => group.SpellOrItem)
            .Select(select => select.ToList());

        TimeSpan.TryParse(combat.Duration, out var durationTime);

        var lessDetails = new List<ResourceRecoveryGeneral>();
        foreach (var item in spells)
        {
            var resourceRecoveryGeneral = new ResourceRecoveryGeneral
            {
                Value = item.Sum(x => x.Value),
                ResourcePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                AverageValue = item.Average(x => x.Value),
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
