using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Details;

public class CombatDetailsDeaths
{
    private readonly ILogger _logger;
    private readonly List<CombatPlayer> _players;
    private readonly Combat _combat;

    public CombatDetailsDeaths(ILogger logger, List<CombatPlayer> players, Combat combat) : base()
    {
        _logger = logger;
        _players = players;
        _combat = combat;
    }

    public int GetData(string player, List<string> combatData)
    {
        int deaths = 0;
        foreach (var item in combatData)
        {
            if (item.Contains(CombatLogKeyWords.UnitDied))
            {
                if (_combat.DeathInfo == null)
                {
                    _combat.DeathInfo = new List<PlayerDeath>();
                }

                deaths += GetPlayersStatus(item);
            }
        }

        return deaths;
    }

    private int GetPlayersStatus(string combatData)
    {
        var isFound = false;

        try
        {
            foreach (var item in _players)
            {
                if (!combatData.Contains(item.Username))
                {
                    continue;
                }

                isFound = true;

                var getTime = combatData.Split(' ')[1];
                _combat.DeathInfo.Add(new PlayerDeath
                {
                    Username = item.Username,
                    Date = DateTimeOffset.Parse(getTime),
                });

                break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, _players);
        }

        return isFound ? 1 : 0;
    }
}
