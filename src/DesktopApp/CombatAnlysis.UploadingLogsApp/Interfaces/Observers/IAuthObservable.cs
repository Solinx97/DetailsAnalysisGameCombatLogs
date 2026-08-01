namespace CombatAnalysis.UploadingLogsApp.Interfaces.Observers;

public interface IAuthObservable
{
    void AddObserver(IAuthObserver o);

    void RemoveObserver(IAuthObserver o);

    void NotifyAuthObservers();
}
