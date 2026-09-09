using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW_12_1_0.CombatParser.Details;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Extensions;

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
                if (combatDetails.DamageDones.TryGetValue(playerId, out var damageCollection))
                {
                    combatDetails.DamageDoneGenerals.TryAdd(playerId, GetDamageDoneGeneral([.. damageCollection.Select(x => x.Value)], duration, false));
                }
                if (combatDetails.HealDones.TryGetValue(playerId, out var healCollection))
                {
                    combatDetails.HealDoneGenerals.TryAdd(playerId, GetHealDoneGeneral([.. healCollection.Select(x => x.Value)], duration));
                }
                if (combatDetails.DamageTakens.TryGetValue(playerId, out var damageTakenCollection))
                {
                    combatDetails.DamageTakenGenerals.TryAdd(playerId, GetDamageDoneGeneral([.. damageTakenCollection.Select(x => x.Value)], duration));
                }
                if (combatDetails.ResourcesRecoveries.TryGetValue(playerId, out var resourceCollection))
                {
                    combatDetails.ResourcesRecoveryGenerals.TryAdd(playerId, GetResourceRecoveryGeneral([.. resourceCollection.Select(x => x.Value)], duration));
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

    private static List<DamageDoneGeneral> GetDamageDoneGeneral(List<DamageDone> collection, string duration, bool isPlayerTarget = true)
    {
        var damageDoneCollection = collection
            .GroupBy(group => group.GameSpellId)
            .Select(select => select.ToList()).ToList();

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return [];
        }

        var lessDetails = new List<DamageDoneGeneral>();
        foreach (var item in damageDoneCollection)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var damagePerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var damagePerSecondRound = double.Round(damagePerSecond, 2);
            var critNumber = item.Where(x => x.DamageType == (int)DamageModificationType.Crit).Count();
            var missNumber = item.Where(x => x.DamageType != (int)DamageModificationType.Crit && x.DamageType != (int)DamageModificationType.Normal).Count();

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

    private static List<HealDoneGeneral> GetHealDoneGeneral(List<HealDone> collection, string duration)
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
            var critNumber = item.Where(x => x.IsCrit).Count();

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

    private static List<ResourceRecoveryGeneral> GetResourceRecoveryGeneral(List<ResourceRecovery> collection, string duration)
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
