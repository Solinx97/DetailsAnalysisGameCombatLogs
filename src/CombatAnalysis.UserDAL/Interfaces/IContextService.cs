namespace CombatAnalysis.UserDAL.Interfaces;

public interface IContextService
{
    Task BeginAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
