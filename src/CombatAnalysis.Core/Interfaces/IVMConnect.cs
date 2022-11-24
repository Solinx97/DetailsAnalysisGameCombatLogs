namespace CombatAnalysis.Core.Interfaces;

public interface IViewModelConnect
{
    object Data { get; set; }

    void PropertyUpdate<T>(object context, string propertyName, object value) where T : class;
}

public interface IViewModelConnect<T>
    where T : notnull
{
    T Data { get; set; }

    void PropertyUpdate<T>(object context, string propertyName, object value) where T : class;
}