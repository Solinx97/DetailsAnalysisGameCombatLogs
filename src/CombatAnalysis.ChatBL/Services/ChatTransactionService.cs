using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Interfaces;

namespace CombatAnalysis.ChatBL.Services;

internal class ChatTransactionService : IChatTransactionService
{
    private readonly IContextService _context;

    public ChatTransactionService(IContextService context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.BeginAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.RollbackAsync();
    }
}
