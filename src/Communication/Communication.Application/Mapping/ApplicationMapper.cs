using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Post;
using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;

namespace Communication.Application.Mapping;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
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
