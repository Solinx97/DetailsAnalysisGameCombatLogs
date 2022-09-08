using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Patterns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CombatAnalysis.CombatParser.Extensions
{
    public static class CombatDetailsExtension
    {
        public static ObservableCollection<DamageDoneGeneral> GetDamageDoneGeneral(this CombatDetailsTemplate extension, List<DamageDone> collection, Combat combat)
        {
            var spells = collection
                .GroupBy(group => group.SpellOrItem)
                .Select(select => select.ToList());

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
                var damageDone = new HealDoneGeneral
                {
                    Value = item.Sum(x => x.Value),
                    HealPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                    AverageValue = item.Average(x => x.Value),
                    MinValue = item.Min(x => x.Value),
                    MaxValue = item.Max(x => x.Value),
                    SpellOrItem = item[0].SpellOrItem,
                    CastNumber = item.Count,
                };

                lessDetails.Add(damageDone);
            }

            lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();
            var damageDoneGroupBySpellOrItem = new ObservableCollection<HealDoneGeneral>(lessDetails);

            return damageDoneGroupBySpellOrItem;
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
                var damageDone = new DamageTakenGeneral
                {
                    Value = item.Sum(x => x.Value),
                    DamageTakenPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                    AverageValue = item.Average(x => x.Value),
                    MinValue = item.Min(x => x.Value),
                    MaxValue = item.Max(x => x.Value),
                    SpellOrItem = item[0].SpellOrItem,
                    CastNumber = item.Count,
                };

                lessDetails.Add(damageDone);
            }

            lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();
            var damageDoneGroupBySpellOrItem = new ObservableCollection<DamageTakenGeneral>(lessDetails);

            return damageDoneGroupBySpellOrItem;
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
                var damageDone = new ResourceRecoveryGeneral
                {
                    Value = item.Sum(x => (int)x.Value),
                    ResourcePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                    AverageValue = item.Average(x => x.Value),
                    MinValue = item.Min(x => (int)x.Value),
                    MaxValue = item.Max(x => (int)x.Value),
                    SpellOrItem = item[0].SpellOrItem,
                    CastNumber = item.Count,
                };

                lessDetails.Add(damageDone);
            }

            lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();
            var damageDoneGroupBySpellOrItem = new ObservableCollection<ResourceRecoveryGeneral>(lessDetails);

            return damageDoneGroupBySpellOrItem;
        }
    }
}
