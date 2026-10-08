namespace Chat.Infrastructure.Interfaces;

public interface IIntegrationEvent
{
    Guid EventId { get; }
}
