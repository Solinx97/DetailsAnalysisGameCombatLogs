using Notification.Application.DTOs;

namespace CombatAnalysis.NotificationAPI.Interfaces;

public interface IKafkaProducerService : IDisposable
{
    Task ProduceAsync(OutboxMessageDto outboxMessage, CancellationToken stoppingToken);
}
