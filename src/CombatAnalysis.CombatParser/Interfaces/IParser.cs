using CombatAnalysis.CombatParser.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParser.Interfaces
{
    public interface IParser : IObservable
    {
        List<Combat> Combats { get; }

        Task Parse(string combatLog);
    }
}