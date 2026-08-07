using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationAPI.Models.Post;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Post;

namespace CombatAnalysis.CommunicationAPI.Mapping;

internal class CommunicationApiMapper : Profile
{
    public CommunicationApiMapper()
    {
        CreateMap<CommunityDto, CommunityModel>().ReverseMap();
        CreateMap<CommunityDiscussionDto, CommunityDiscussionModel>().ReverseMap();
        CreateMap<CommunityDiscussionCommentDto, CommunityDiscussionCommentModel>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUserModel>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunityModel>().ReverseMap();
        CreateMap<UserPostDto, UserPostModel>().ReverseMap();
        CreateMap<UserPostCommentDto, UserPostCommentModel>().ReverseMap();
        CreateMap<UserPostLikeDto, UserPostLikeModel>().ReverseMap();
        CreateMap<UserPostDislikeDto, UserPostDislikeModel>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPostModel>().ReverseMap();
        CreateMap<CommunityPostCommentDto, CommunityPostCommentModel>().ReverseMap();
        CreateMap<CommunityPostLikeDto, CommunityPostLikeModel>().ReverseMap();
        CreateMap<CommunityPostDislikeDto, CommunityPostDislikeModel>().ReverseMap();
    }
}
