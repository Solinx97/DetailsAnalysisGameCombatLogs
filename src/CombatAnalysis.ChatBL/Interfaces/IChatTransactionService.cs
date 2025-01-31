namespace CombatAnalysis.ChatBL.Interfaces;

public interface IChatTransactionService
{
    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
