using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Mappers.Profiles;

public class ApplicationChatProfile : Profile
{
    public ApplicationChatProfile()
    {
        ValueObjectMap();

        CreateMap<VoiceChatDto, VoiceChat>()
                 .ConstructUsing(dto => new VoiceChat(
                     dto.Id,
                     dto.AppUserId
                 )).ReverseMap();

        CreateMap<PersonalChatDto, PersonalChat>().ReverseMap();

        CreateMap<PersonalChatMessageDto, PersonalChatMessage>().ReverseMap();

        CreateMap<GroupChatDto, GroupChat>().ReverseMap();

        CreateMap<GroupChatRulesDto, GroupChatRules>().ReverseMap();

        CreateMap<Domain.DTOs.GroupChatMessageDto, GroupChatMessageDto>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessage>().ReverseMap();
        
        CreateMap<GroupChatUserDto, GroupChatUser>().ReverseMap();
    }

    private void ValueObjectMap()
    {
        CreateMap<GroupChatId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, GroupChatId>()
            .ConvertUsing(src => new GroupChatId(src));

        CreateMap<GroupChatUserId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, GroupChatUserId>()
            .ConvertUsing(src => new GroupChatUserId(src));

        CreateMap<GroupChatMessageId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, GroupChatMessageId>()
            .ConvertUsing(src => new GroupChatMessageId(src));

        CreateMap<GroupChatRulesId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, GroupChatRulesId>()
            .ConvertUsing(src => new GroupChatRulesId(src));

        CreateMap<PersonalChatId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, PersonalChatId>()
            .ConvertUsing(src => new PersonalChatId(src));

        CreateMap<PersonalChatMessageId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, PersonalChatMessageId>()
            .ConvertUsing(src => new PersonalChatMessageId(src));

        CreateMap<UserId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, UserId>()
            .ConvertUsing(src => new UserId(src));

        CreateMap<string, VoiceChatId>()
            .ConvertUsing(src => new VoiceChatId(src));

        CreateMap<VoiceChatId, string>()
            .ConvertUsing(src => new VoiceChatId(src));
    }
}
