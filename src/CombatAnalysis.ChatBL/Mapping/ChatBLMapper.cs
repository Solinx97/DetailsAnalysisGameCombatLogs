using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatDAL.Entities;

namespace CombatAnalysis.ChatBL.Mapping;

public class ChatBLMapper : Profile
{
    public ChatBLMapper()
    {
        CreateMap<VoiceChatDto, VoiceChat>().ReverseMap();
        CreateMap<PersonalChatDto, PersonalChat>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessage>().ReverseMap();
        CreateMap<PersonalChatMessageCountDto, PersonalChatMessageCount>().ReverseMap();
        CreateMap<GroupChatDto, GroupChat>().ReverseMap();
        CreateMap<GroupChatRulesDto, GroupChatRules>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessage>().ReverseMap();
        CreateMap<UnreadGroupChatMessageDto, UnreadGroupChatMessage>().ReverseMap();
        CreateMap<GroupChatMessageCountDto, GroupChatMessageCount>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUser>().ReverseMap();
    }
}
