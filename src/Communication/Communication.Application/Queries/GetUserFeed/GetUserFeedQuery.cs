using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetUserFeed;

public record GetUserFeedQuery(
    string AppUserId,
    List<string> FriendsId,
    int Page, int PageSize
    ) : IRequest<AllUserFeedDto>;
