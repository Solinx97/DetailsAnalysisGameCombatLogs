using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.Entities.Post;
using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;

namespace Communication.Application.Mapping;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        ValueObjectMap();

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

    private void ValueObjectMap()
    {
        CreateMap<CommunityId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityId>()
            .ConvertUsing(src => new CommunityId(src));

        CreateMap<UserId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, UserId>()
            .ConvertUsing(src => new UserId(src));

        CreateMap<CommunityPostId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityPostId>()
            .ConvertUsing(src => new CommunityPostId(src));

        CreateMap<CommunityPostCommentId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityPostCommentId>()
            .ConvertUsing(src => new CommunityPostCommentId(src));

        CreateMap<CommunityPostDislikeId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityPostDislikeId>()
            .ConvertUsing(src => new CommunityPostDislikeId(src));

        CreateMap<CommunityPostLikeId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityPostLikeId>()
            .ConvertUsing(src => new CommunityPostLikeId(src));

        CreateMap<CommunityUserId, Guid>()
            .ConvertUsing(src => src.Value);

        CreateMap<Guid, CommunityUserId>()
            .ConvertUsing(src => new CommunityUserId(src));

        CreateMap<InviteToCommunityId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, InviteToCommunityId>()
            .ConvertUsing(src => new InviteToCommunityId(src));

        CreateMap<CommunityDiscussionId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityDiscussionId>()
            .ConvertUsing(src => new CommunityDiscussionId(src));

        CreateMap<CommunityDiscussionCommentId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, CommunityDiscussionCommentId>()
            .ConvertUsing(src => new CommunityDiscussionCommentId(src));

        CreateMap<UserPostId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, UserPostId>()
            .ConvertUsing(src => new UserPostId(src));

        CreateMap<UserPostCommentId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, UserPostCommentId>()
            .ConvertUsing(src => new UserPostCommentId(src));

        CreateMap<UserPostDislikeId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, UserPostDislikeId>()
            .ConvertUsing(src => new UserPostDislikeId(src));

        CreateMap<UserPostLikeId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, UserPostLikeId>()
            .ConvertUsing(src => new UserPostLikeId(src));
    }
}
