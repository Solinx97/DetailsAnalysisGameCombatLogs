namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IContextService
{
    Task BeginAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
