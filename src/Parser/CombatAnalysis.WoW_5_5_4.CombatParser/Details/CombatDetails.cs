using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Details;

public class CombatDetails(ICombatParserHelper combatParserHelper, ILogger logger, ConcurrentDictionary<string, Unit> units) 
    : WoW.CombatParser.Details.CombatDetails(combatParserHelper, logger, units)
{
    protected override void Parse(string[] playersId, string combatDataLine, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        var hasCasts = _casts.Any(combatDataLine.Contains);
        var hasPositions = _positions.Any(combatDataLine.Contains);
        var hasDieds = _dieds.Any(combatDataLine.Contains);
        var hasAuras = _auras.Any(combatDataLine.Contains);
        var hasHealth = _health.Any(combatDataLine.Contains);
        var hasDamage = _damageVariations.Any(combatDataLine.Contains);
        var hasHeal = _healVariations.Any(combatDataLine.Contains);
        var hasAbsorb = _absorbVariations.Any(combatDataLine.Contains);
        var hasResources = _resourceVariations.Any(combatDataLine.Contains);

        if (!hasCasts && !hasPositions && !hasDieds && !hasAuras 
            && hasHealth && !hasDamage && !hasHeal && !hasAbsorb && !hasResources)
        {
            return;
        }

        var splitCombatData = _combatParserHelper.SplitCombatData(combatDataLine);
        var combatDetailsManager = new CombatDetailsManager(_combatParserHelper, combatStarted, combatFinished);

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
                    if (hasHealth)
                    {
                        CalculateHealthes(combatDetailsManager, splitCombatData);
                    }
                },
                () =>
                {
                    if (hasDamage)
                    {
                        CalculateDamageTaken(combatDetailsManager, splitCombatData, playersId);
                    }
                },
                () =>
                {
                    CalculateGeneral(combatDataLine, combatDetailsManager, splitCombatData, playersId);
                }
            );
    }
}
