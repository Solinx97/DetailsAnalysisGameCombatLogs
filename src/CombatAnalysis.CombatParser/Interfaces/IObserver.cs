using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface IObserver
{
    void Update(Combat data);
}
