using CombatAnalysis.Core.Enums;

namespace CombatAnalysis.Core.Interfaces.Observers
{
    public interface IResponseStatusObserver
    {
        void Update(ResponseStatus status);
    }
}
