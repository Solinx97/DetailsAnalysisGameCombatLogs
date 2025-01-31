namespace CombatAnalysis.DAL.Interfaces;

public interface IContextService
{
    Task BeginAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
