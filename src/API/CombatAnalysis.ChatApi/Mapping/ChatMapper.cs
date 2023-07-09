using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Models.Community;

namespace CombatAnalysis.ChatApi.Mapping;

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
        CreateMap<CommunityDto, CommunityModel>().ReverseMap();
        CreateMap<CommunityPostCommentDto, CommunityPostCommentModel>().ReverseMap();
        CreateMap<CommunityPostDislikeDto, CommunityPostDislikeModel>().ReverseMap();
        CreateMap<CommunityPostLikeDto, CommunityPostLikeModel>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPostModel>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUserModel>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunityModel>().ReverseMap();
    }
}
