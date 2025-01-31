namespace CombatAnalysis.BL.Interfaces;

public interface ICombatTransactionService
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
