using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetUserFeed;

public record GetUserFeedQuery(
    string AppUserId,
    int Page, int PageSize
    ) : IRequest<IEnumerable<UserFeedDto>>;
