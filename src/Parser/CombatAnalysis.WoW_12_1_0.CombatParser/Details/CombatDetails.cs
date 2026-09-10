using Microsoft.Extensions.Logging;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Details;

public class CombatDetails(ILogger logger) : WoW.CombatParser.Details.CombatDetails(logger)
{
    public CombatDetails(ILogger logger, Dictionary<string, List<string>> petsId) : this(logger)
    {
        _petsId = petsId;
    }

    protected override void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasSummon = _summon.Any(combatDataLine.Contains);
        var hasCasts = _casts.Any(combatDataLine.Contains);
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasSummon && !hasCasts && !hasPositions && !hasDieds
            && !hasAuras && !hasHeal && !hasDamage && !hasAbsorb && !hasResources)
        {
            return;
        }

        var splitCombatData = SplitCombatData(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(playersId, combatStarted, combatFinished);

        if (hasSummon)
        {
            combatDetailsManager.GetSummonUnit(splitCombatData, Units);
        }

        Parallel.Invoke(
                () =>
                {
                    if (hasCasts)
                    {
                        CalculateCasts(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    if (hasPositions)
                    {
                        CalculatePositions(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    if (hasDamage)
                    {
                        CalculateDamageTaken(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    CalculateGeneral(combatDataLine, combatDetailsManager, splitCombatData, playersId);
                }
            );
    }
}
