using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface IParser<TModel> : IObservable<TModel>
    where TModel : class
{
    List<Combat> Combats { get; }

    Dictionary<string, List<string>> PetsId { get; }

    Task<bool> FileCheck(string combatLog);

    Task Parse(string combatLog);

    void GetPlayerInfo(Dictionary<string, string> specs, Dictionary<string, string> classes);
}