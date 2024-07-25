namespace CombatAnalysis.CombatParser.Interfaces;

public interface IObserver<TModel>
    where TModel : class
{
    void Update(TModel data);
}
