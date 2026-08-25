using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Communication.Domain.ReadModel;

namespace Communication.Application.Mapping;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<CommunityDto, Community>().ReverseMap();
        CreateMap<CommunityDiscussionDto, CommunityDiscussion>().ReverseMap();
        CreateMap<CommunityDiscussionCommentDto, CommunityDiscussionComment>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUser>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunity>().ReverseMap();
        CreateMap<UserFeedDto, UserFeedReadModel>().ReverseMap();
        CreateMap<UserPostLikeDto, UserPostLike>().ReverseMap();
        CreateMap<UserPostDislikeDto, UserPostDislike>().ReverseMap();
        CreateMap<UserPostCommentDto, UserPostComment>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPost>().ReverseMap();
        CreateMap<CommunityPostCommentDto, CommunityPostComment>().ReverseMap();
        CreateMap<CommunityPostLikeDto, CommunityPostLike>().ReverseMap();
        CreateMap<CommunityPostDislikeDto, CommunityPostDislike>().ReverseMap();

        CreateMap<CommunityPost, CommunityPostDto>();
        CreateMap<CommunityPostReadModel, CommunityPostDto>();
        CreateMap<UserPost, UserPostDto>();
        CreateMap<UserPostReadModel, UserPostDto>();
    }
}
