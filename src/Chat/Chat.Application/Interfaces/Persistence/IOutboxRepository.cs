using Chat.Application.DTOs;

namespace Chat.Application.Interfaces.Persistence;

public interface IOutboxRepository
{
    Task AddAsync(Guid eventId, string eventType, string topic, string key, string payload, CancellationToken cancelationToken);

    Task<IEnumerable<OutboxMessageDto>> GetPendingAsync(int batchSize, CancellationToken cancellationToken);

    Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken);
}
