namespace CombatAnalysis.CombatParser.Interfaces;

public interface IObservable<TModel>
    where TModel : class
{
    void AddObserver(IObserver<TModel> o);

    void RemoveObserver(IObserver<TModel> o);

    void NotifyObservers();
}
