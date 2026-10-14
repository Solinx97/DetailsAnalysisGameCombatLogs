using MediatR;

namespace Communication.Application.Queries.CountFeedNewPosts;

public record CountFeedNewPostsQuery(
    Guid AppUserId,
    List<Guid> FriendsId,
    DateTimeOffset LastCheck
    ) : IRequest<int>;
