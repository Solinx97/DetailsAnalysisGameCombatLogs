using AutoMapper;
using CombatAnalysis.CommunicationAPI.Models.Community;
using CombatAnalysis.CommunicationAPI.Models.Post;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;

namespace CombatAnalysis.CommunicationAPI.Mapping;

public class CommunicationMapper : Profile
{
    public CommunicationMapper()
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
