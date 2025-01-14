namespace CombatAnalysis.ChatDAL.Interfaces;

public interface IContextService
{
    Task BeginAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
