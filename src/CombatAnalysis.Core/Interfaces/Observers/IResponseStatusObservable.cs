namespace CombatAnalysis.Core.Interfaces.Observers
{
    public interface IResponseStatusObservable
    {
        void AddObserver(IResponseStatusObserver o);

        void RemoveObserver(IResponseStatusObserver o);

        void NotifyResponseStatusObservers();
    }
}
