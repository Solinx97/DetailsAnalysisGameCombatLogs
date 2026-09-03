using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW_12_1_0.CombatParser.Details;
using CombatAnalysis.WoW_12_1_0.CombatParser.Enums;
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
                combatDetails.DamageDoneGenerals.TryAdd(playerId, GetDamageDoneGeneral([.. combatDetails.DamageDones[playerId].Select(x => x.Value)], duration));
                combatDetails.HealDoneGenerals.TryAdd(playerId, GetHealDoneGeneral([.. combatDetails.HealDones[playerId].Select(x => x.Value)], duration));
                combatDetails.DamageTakenGenerals.TryAdd(playerId, GetDamageTakenGeneral([.. combatDetails.DamageTakens[playerId].Select(x => x.Value)], duration));
                combatDetails.ResourcesRecoveryGenerals.TryAdd(playerId, GetResourceRecoveryGeneral([.. combatDetails.ResourcesRecoveries[playerId].Select(x => x.Value)], duration));
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

    private static List<DamageDoneGeneral> GetDamageDoneGeneral(List<DamageDone> collection, string duration)
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
            var critNumber = item.Where(x => x.DamageType == (int)DamageType.Crit).Count();
            var missNumber = item.Where(x => x.DamageType != (int)DamageType.Crit && x.DamageType != (int)DamageType.Normal).Count();
            var isPet = item.FirstOrDefault()?.IsPet ?? false;

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
                IsPet = isPet,
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

    private static List<DamageTakenGeneral> GetDamageTakenGeneral(List<DamageTaken> collection, string duration)
    {
        var spells = collection
            .GroupBy(group => group.GameSpellId)
            .Select(select => select.ToList());

        if (!TimeSpan.TryParse(duration, out var durationTime))
        {
            return [];
        }

        var lessDetails = new List<DamageTakenGeneral>();
        foreach (var item in spells)
        {
            var averageValue = double.Round(item.Average(x => x.Value), 2);
            var damageTakenPerSecond = item.Sum(x => x.Value) / durationTime.TotalSeconds;
            var damageTakenPerSecondRound = double.Round(damageTakenPerSecond, 2);

            var damageTakenGeneral = new DamageTakenGeneral
            {
                GameSpellId = item[0].GameSpellId,
                Spell = item[0].Spell,
                Value = item.Sum(x => x.Value),
                ActualValue = item.Sum(x => x.ActualValue),
                DamageTakenPerSecond = damageTakenPerSecondRound,
                AverageValue = averageValue,
                MinValue = item.Min(x => x.Value),
                MaxValue = item.Max(x => x.Value),
                CastNumber = item.Count,
            };

            lessDetails.Add(damageTakenGeneral);
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
