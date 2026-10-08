using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class OutboxRepository(ChatContext context) : IOutboxRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(OutboxMessage outbox, CancellationToken cancelationToken)
    {
        await _context.OutboxMessages
                    .AddAsync(outbox, cancelationToken);
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken)
    {
        return await _context.OutboxMessages
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public void MarkAsProcessed(OutboxMessage message)
    {
        message.ProcessedAt = DateTime.UtcNow;
    }
}