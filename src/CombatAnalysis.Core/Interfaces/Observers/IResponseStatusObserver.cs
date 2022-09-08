using CombatAnalysis.Core.Core;

namespace CombatAnalysis.Core.Interfaces.Observers
{
    public interface IResponseStatusObserver
    {
        void Update(ResponseStatus status);
    }
}
