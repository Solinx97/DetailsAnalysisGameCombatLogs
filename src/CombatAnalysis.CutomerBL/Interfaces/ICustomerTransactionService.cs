namespace CombatAnalysis.CustomerBL.Interfaces;

public interface ICustomerTransactionService
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
