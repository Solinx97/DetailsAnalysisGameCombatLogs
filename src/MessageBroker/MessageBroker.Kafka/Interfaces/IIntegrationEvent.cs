namespace MessageBroker.Kafka.Interfaces;

public interface IIntegrationEvent
{
    Guid EventId { get; }
}
