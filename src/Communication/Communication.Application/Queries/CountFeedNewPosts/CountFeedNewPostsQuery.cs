using MediatR;

namespace Communication.Application.Queries.CountFeedNewPosts;

public record CountFeedNewPostsQuery(
    string AppUserId,
    List<string> FriendsId,
    DateTimeOffset LastCheck
    ) : IRequest<int>;
