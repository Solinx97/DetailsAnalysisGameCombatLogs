using MediatR;

namespace Communication.Application.Queries.CountCommunityPostLike;

public record CountCommunityPostLikeQuery(
    int CommunityPostId
    ) : IRequest<int>;
