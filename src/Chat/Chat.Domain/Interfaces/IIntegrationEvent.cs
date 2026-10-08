namespace Chat.Domain.Interfaces;

internal interface IIntegrationEvent
{
    Guid EventId { get; }
}
