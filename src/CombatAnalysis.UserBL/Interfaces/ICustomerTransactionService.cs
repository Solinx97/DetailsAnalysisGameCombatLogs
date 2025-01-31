namespace CombatAnalysis.UserBL.Interfaces;

public interface ICustomerTransactionService
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
