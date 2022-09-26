using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Mapping
{
    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
            CreateMap<PersonalChatMessageDto, PersonalChatMessageModel>().ReverseMap();
            CreateMap<InviteToGroupChatDto, InviteToGroupChatModel>().ReverseMap();
            CreateMap<GroupChatDto, GroupChatModel>().ReverseMap();
            CreateMap<GroupChatMessageDto, GroupChatMessageModel>().ReverseMap();
            CreateMap<GroupChatUserDto, GroupChatUserModel>().ReverseMap();
            CreateMap<BannedUserDto, BannedUserModel>().ReverseMap();
        }
    }
}
