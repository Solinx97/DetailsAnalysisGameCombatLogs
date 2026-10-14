using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetUserFeed;

public record GetUserFeedQuery(
    Guid AppUserId,
    List<Guid> FriendsId,
    int Page,
    int PageSize
    ) : IRequest<AllUserFeedDto>;
