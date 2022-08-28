using CombatAnalysis.Core.Core;

namespace CombatAnalysis.Core.Interfaces
{
    public interface IResponseStatusObserver
    {
        void Update(ResponseStatus status);
    }
}
