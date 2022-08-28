namespace CombatAnalysis.Core.Interfaces
{
    public interface IResponseStatusObservable
    {
        void AddObserver(IResponseStatusObserver o);
        
        void RemoveObserver(IResponseStatusObserver o);

        void NotifyObservers();
    }
}
