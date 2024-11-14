namespace CombatAnalysis.CustomerDAL.Interfaces;

public interface IContextService
{
    Task BeginAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
