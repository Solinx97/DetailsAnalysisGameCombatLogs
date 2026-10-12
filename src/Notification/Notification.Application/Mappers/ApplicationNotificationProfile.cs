using AutoMapper;
using Notification.Application.DTOs;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Mappers;

public class ApplicationNotificationProfile : Profile
{
    public ApplicationNotificationProfile()
    {
        ValueObjectMap();

        CreateMap<NotificationDto, Domain.Aggregates.Notification>().ReverseMap();
    }

    private void ValueObjectMap()
    {
        CreateMap<NotificationId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, NotificationId>()
            .ConvertUsing(src => new NotificationId(src));

        CreateMap<UserId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, UserId>()
            .ConvertUsing(src => new UserId(src));
    }
}
