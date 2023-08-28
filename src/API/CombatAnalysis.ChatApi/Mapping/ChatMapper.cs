using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatApi.Models.Community;
using CombatAnalysis.ChatApi.Models.Post;

namespace CombatAnalysis.ChatApi.Mapping;

public class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessageModel>().ReverseMap();
        CreateMap<PersonalChatMessageCountDto, PersonalChatMessageCountModel>().ReverseMap();
        CreateMap<GroupChatDto, GroupChatModel>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessageModel>().ReverseMap();
        CreateMap<GroupChatMessageCountDto, GroupChatMessageCountModel>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUserModel>().ReverseMap();
        CreateMap<CommunityDto, CommunityModel>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUserModel>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunityModel>().ReverseMap();
        CreateMap<PostDto, PostModel>().ReverseMap();
        CreateMap<PostLikeDto, PostLikeModel>().ReverseMap();
        CreateMap<PostDislikeDto, PostDislikeModel>().ReverseMap();
        CreateMap<PostCommentDto, PostCommentModel>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPostModel>().ReverseMap();
        CreateMap<UserPostDto, UserPostModel>().ReverseMap();
    }
}
