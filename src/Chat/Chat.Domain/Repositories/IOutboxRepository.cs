using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IOutboxRepository
{
    Task AddAsync(OutboxMessage outbox, CancellationToken cancelationToken);

    Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken);

    void MarkAsProcessed(OutboxMessage message);
}
