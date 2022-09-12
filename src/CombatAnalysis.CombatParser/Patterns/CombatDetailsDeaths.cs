using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Patterns
{
    public class CombatDetailsDeaths : CombatDetailsTemplate
    {
        private readonly ILogger _logger;
        private readonly List<CombatPlayer> _players;

        public CombatDetailsDeaths(ILogger logger, List<CombatPlayer> players) : base()
        {
            _logger = logger;
            _players = players;
        }

        public override int GetData(string player, List<string> combatData)
        {
            int deaths = 0;
            foreach (var item in combatData)
            {
                if (item.Contains("UNIT_DIED"))
                {
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
                    if (combatData.Contains(item.UserName))
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message, _players);
            }

            return isFound ? 1 : 0;
        }
    }
}
