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
        CreateMap<PostDto, Post>().ReverseMap();
        CreateMap<PostLikeDto, PostLike>().ReverseMap();
        CreateMap<PostDislikeDto, PostDislike>().ReverseMap();
        CreateMap<PostCommentDto, PostComment>().ReverseMap();
        CreateMap<UserPostDto, UserPost>().ReverseMap();
    }
}
