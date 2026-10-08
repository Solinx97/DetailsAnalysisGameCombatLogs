using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IOutboxRepository
{
    Task<IReadOnlyList<OutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken);

    void MarkAsProcessed(OutboxMessage message);
}
