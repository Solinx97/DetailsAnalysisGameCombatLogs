using Chat.Application.DTOs;

namespace CombatAnalysis.ChatAPI.Interfaces;

public interface IKafkaProducerService : IDisposable
{
    Task ProduceAsync(OutboxMessageDto outboxMessage, CancellationToken stoppingToken);
}
