using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsPositions : BaseCombatDetails
{
    private readonly string[] _healVariations = new string[]
    {
        CombatLogKeyWords.SpellHeal,
    };
    private readonly string[] _damageVariations = new string[]
    {
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SwingDamageLanded,
    };
    private readonly ILogger _logger;

    public CombatDetailsPositions(ILogger logger) : base()
    {
        _logger = logger;
        Positions = new List<CombatPlayerPosition>();
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        try
        {
            if (playerId == null)
            {
                throw new ArgumentNullException(playerId);
            }

            GetSummaryPositions(playerId, combatData);

            return 1;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return 0;
        }
    }

    private void GetSummaryPositions(string playerId, List<string> combatData)
    {
        var allPositions = new List<List<float>>();

        foreach (var item in combatData)
        {
            var itemHasHealVariation = _healVariations.Any(item.Contains);
            var itemHasDamageVariation = _damageVariations.Any(item.Contains);
            if ((itemHasHealVariation || itemHasDamageVariation) && item.Contains(playerId))
            {
                var usefulInformation = GetUsefulInformation(item);
                var positions = GetPositionsInformation(playerId, usefulInformation);
                if (positions != null)
                {
                    Positions.Add(positions);
                }
            }
        }
    }

    private static CombatPlayerPosition GetPositionsInformation(string playerId, List<string> combatData)
    {
        if (!combatData[2].Equals(playerId))
        {
            return null;
        }

        var pos1Index = 24;
        var pos2Index = 25;

        if (combatData[1].Equals(CombatLogKeyWords.SwingDamage)
            || combatData[1].Equals(CombatLogKeyWords.SwingDamageLanded))
        {
            pos1Index = 21;
            pos2Index = 22;
        }

        if (double.TryParse(combatData[pos1Index], out var position1)
            && double.TryParse(combatData[pos2Index], out var position2))
        {
            var position = new CombatPlayerPosition
            {
                PositionX = position1,
                PositionY = position2
            };

            return position;
        }

        return null;
    }
}
