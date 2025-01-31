using AutoMapper;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;

namespace CombatAnalysis.ChatApi.Mapping;

internal class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<VoiceChatDto, VoiceChatModel>().ReverseMap();
        CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessageModel>().ReverseMap();
        CreateMap<PersonalChatMessageCountDto, PersonalChatMessageCountModel>().ReverseMap();
        CreateMap<GroupChatDto, GroupChatModel>().ReverseMap();
        CreateMap<GroupChatRulesDto, GroupChatRulesModel>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessageModel>().ReverseMap();
        CreateMap<UnreadGroupChatMessageDto, UnreadGroupChatMessageModel>().ReverseMap();
        CreateMap<GroupChatMessageCountDto, GroupChatMessageCountModel>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUserModel>().ReverseMap();
    }
}
