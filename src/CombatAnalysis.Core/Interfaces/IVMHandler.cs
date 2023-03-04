namespace CombatAnalysis.Core.Interfaces;

public interface IVMHandler
{
    void PropertyUpdate<T1>(object context, string propertyName, object value) where T1 : class;
}