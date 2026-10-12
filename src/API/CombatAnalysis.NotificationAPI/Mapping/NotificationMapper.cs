using AutoMapper;
using CombatAnalysis.NotificationAPI.Models;
using Notification.Application.DTOs;

namespace CombatAnalysis.NotificationAPI.Mapping;

internal class NotificationMapper : Profile
{
    public NotificationMapper()
    {
        CreateMap<NotificationDto, NotificationModel>().ReverseMap();
    }
}
