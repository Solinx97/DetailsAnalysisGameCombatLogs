using MediatR;

namespace Communication.Application.Queries.CountCommunityNewPosts;

public record CountCommunityNewPostsQuery(
    int CommunityId,
    DateTimeOffset LastCheck
    ) : IRequest<int>;
