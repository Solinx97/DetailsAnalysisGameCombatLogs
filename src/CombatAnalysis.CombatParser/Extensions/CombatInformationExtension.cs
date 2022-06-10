using CombatAnalysis.CombatParser.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CombatAnalysis.CombatParser.Extensions
{
    public static class CombatInformationExtension
    {
        public static ObservableCollection<DamageDoneGeneral> GetDamageDoneGeneral(this CombatInformation extension, List<DamageDone> collection, Combat combat)
        {
            var spells = collection
                .GroupBy(group => group.SpellOrItem)
                .Select(select => select.ToList())
                .ToList();

            TimeSpan.TryParse(combat.Duration, out var durationTime);

            var lessDetails = new List<DamageDoneGeneral>();
            foreach (var item in spells)
            {
                var damageDone = new DamageDoneGeneral
                {
                    Value = item.Sum(x => x.Value),
                    DamagePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds,
                    AverageValue = item.Average(x => x.Value),
                    MinValue = item.Min(x => x.Value),
                    MaxValue = item.Max(x => x.Value),
                    SpellOrItem = item[0].SpellOrItem,
                    CastNumber = item.Count,
                };

                lessDetails.Add(damageDone);
            }

            lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();
            var damageDoneGroupBySpellOrItem = new ObservableCollection<DamageDoneGeneral>(lessDetails);

            return damageDoneGroupBySpellOrItem;
        }
    }
}
