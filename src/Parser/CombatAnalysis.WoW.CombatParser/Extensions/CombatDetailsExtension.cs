using CombatAnalysis.WoW.CombatParser.Details;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.WoW.CombatParser.Extensions;

public static class CombatDetailsExtension
{
    public static void CalculateGeneralData(this CombatDetails combatDetails, string[] playersId, string? duration)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(playersId, nameof(playersId));
            ArgumentException.ThrowIfNullOrEmpty(duration, nameof(duration));
            ArgumentOutOfRangeException.ThrowIfZero(playersId.Length);

            foreach (var playerId in playersId)
            {
                if (combatDetails.Units.TryGetValue(playerId, out var unit))
                {
                    unit.DamageDoneGenerals.AddRange(GetDamageDoneGeneral([.. unit.DamageDones], duration));
                    unit.DamageTakenGenerals.AddRange(GetDamageDoneGeneral([.. unit.DamageTakens], duration, true));
                    unit.HealDoneGenerals.AddRange(GetHealDoneGeneral([.. unit.HealDones], duration));
                    unit.ResourceRecoveryGenerals.AddRange(GetResourceRecoveryGeneral([.. unit.ResourceRecoveries], duration));
                }
            }
        }
        catch (ArgumentNullException ex)
        {   
            combatDetails.Logger.LogError("Some argument was null: {Param}", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            combatDetails.Logger.LogError("Some argument out of valid range: {Param}", ex.ParamName);
        }
    }

    private static List<DamageDoneGeneral> GetDamageDoneGeneral(List<ICombatPlayerResourceRefs> collection, string duration, bool isPlayerTarget = false)
    {
        var damageCollection = collection
            .GroupBy(group => group.GameSpellId)
            .Select(select => select.ToList()).ToList();

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return [];
        }

        var lessDetails = new List<DamageDoneGeneral>();
        foreach (var item in damageCollection)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var damagePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var damagePerSecondRound = double.Round(damagePerSecond, 2);
            var critNumber = item.Where(x => x.ModificationType == (int)ModificationType.Crit).Count();
            var missNumber = item.Where(x => x.ModificationType != (int)ModificationType.Crit 
                                    && x.ModificationType != (int)ModificationType.Normal
                                    && x.ModificationType != (int)ModificationType.Absorb).Count();

            var damageDoneGeneral = new DamageDoneGeneral
            {
                GameSpellId = item[0].GameSpellId,
                Spell = item[0].Spell,
                Value = item.Sum(x => x.Value),
                DamagePerSecond = damagePerSecondRound,
                CritNumber = critNumber,
                MissNumber = missNumber,
                CastNumber = item.Count,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                AverageValue = averageValue,
                IsPlayerTarget = isPlayerTarget
            };

            lessDetails.Add(damageDoneGeneral);
        }

        lessDetails = [.. lessDetails.OrderByDescending(x => x.Value)];

        return lessDetails;
    }

    private static List<HealDoneGeneral> GetHealDoneGeneral(List<ICombatPlayerResourceRefs> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.GameSpellId)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return [];
        }

        var lessDetails = new List<HealDoneGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var healPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var healPerSecondRound = double.Round(healPerSecond, 2);
            var critNumber = item.Where(x => x.ModificationType == (int)ModificationType.Crit).Count();

            var healDoneGeneral = new HealDoneGeneral
            {
                GameSpellId = item[0].GameSpellId,
                Spell = item[0].Spell,
                Value = item.Sum(x => x.Value),
                HealPerSecond = healPerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                CastNumber = item.Count,
                CritNumber = critNumber,
            };

            lessDetails.Add(healDoneGeneral);
        }

        lessDetails = [.. lessDetails.OrderByDescending(x => x.Value)];

        return lessDetails;
    }

    private static List<ResourceRecoveryGeneral> GetResourceRecoveryGeneral(List<ICombatPlayerResourceRefs> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.GameSpellId)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return [];
        }

        var lessDetails = new List<ResourceRecoveryGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var resourcePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var resourcePerSecondRound = double.Round(resourcePerSecond, 2);

            var resourceRecoveryGeneral = new ResourceRecoveryGeneral
            {
                GameSpellId = item[0].GameSpellId,
                Spell = item[0].Spell,
                Value = item.Sum(x => x.Value),
                ResourcePerSecond = resourcePerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                CastNumber = item.Count,
            };

            lessDetails.Add(resourceRecoveryGeneral);
        }

        lessDetails = [.. lessDetails.OrderByDescending(x => x.Value)];

        return lessDetails;
    }
}
