using AutoMapper;
using Chat.Application.DTOs;
using Chat.Infrastructure.Persistence.Outbox;

namespace Chat.Infrastructure.Mappers;

public class InfrastructureChatProfile : Profile
{
    public InfrastructureChatProfile()
    {
        CreateMap<OutboxMessageDto, OutboxMessage>().ReverseMap();
    }
}
