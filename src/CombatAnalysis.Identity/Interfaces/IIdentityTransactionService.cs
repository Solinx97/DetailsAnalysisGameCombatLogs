namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityTransactionService
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
