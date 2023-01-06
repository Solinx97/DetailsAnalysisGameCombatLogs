namespace CombatAnalysis.Core.Interfaces;

public interface IVMDataHandler<T>
    where T : notnull
{
    T Data { get; set; }
}
