using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface IParser : IObservable
{
    List<Combat> Combats { get; }

    Task<bool> FileCheck(string combatLog);

    Task Parse(string combatLog);
}