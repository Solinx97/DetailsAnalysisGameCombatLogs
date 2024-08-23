using AutoMapper;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.DTO.Post;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationBL.Mapping;

public class BLMapper : Profile
{
    public BLMapper()
    {
        CreateMap<CommunityDto, Community>().ReverseMap();
        CreateMap<CommunityDiscussionDto, CommunityDiscussion>().ReverseMap();
        CreateMap<CommunityDiscussionCommentDto, CommunityDiscussionComment>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPost>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUser>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunity>().ReverseMap();
        CreateMap<UserPostDto, UserPost>().ReverseMap();
        CreateMap<UserPostLikeDto, UserPostLike>().ReverseMap();
        CreateMap<UserPostDislikeDto, UserPostDislike>().ReverseMap();
        CreateMap<UserPostCommentDto, UserPostComment>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPost>().ReverseMap();
        CreateMap<CommunityPostCommentDto, CommunityPostComment>().ReverseMap();
        CreateMap<CommunityPostLikeDto, CommunityPostLike>().ReverseMap();
        CreateMap<CommunityPostDislikeDto, CommunityPostDislike>().ReverseMap();
    }
}
