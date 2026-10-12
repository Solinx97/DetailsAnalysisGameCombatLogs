using AutoMapper;
using Notification.Application.DTOs;
using Notification.Infrastructure.Persistence.Outbox;

namespace Notification.Infrastructure.Mappers;

public class InfrastructureNotificationProfile : Profile
{
    public InfrastructureNotificationProfile()
    {
        CreateMap<OutboxMessageDto, OutboxMessage>().ReverseMap();
    }
}
