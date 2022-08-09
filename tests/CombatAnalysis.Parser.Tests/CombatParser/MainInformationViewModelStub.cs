using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Parser.Tests.CombatParser
{
    internal class MainInformationViewModelStub
    {
        private readonly IParser _parser;

        public MainInformationViewModelStub(IParser parser)
        {
            _parser = parser;
        }

        public async Task<List<Combat>> GetCombatDataDetailsAsync(string combatLog)
        {
            await _parser.Parse(combatLog);
            var combats = _parser.Combats;

            for (int i = 0; i < combats.Count; i++)
            {
                foreach (var item in combats[i].Players)
                {
                    combats[i].DamageDone += item.DamageDone;
                    combats[i].HealDone += item.HealDone;
                    combats[i].EnergyRecovery += item.EnergyRecovery;
                    combats[i].DamageTaken += item.DamageTaken;
                }
            }

            return combats;
        }
    }
}
