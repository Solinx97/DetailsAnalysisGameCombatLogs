using MediatR;

namespace Communication.Application.Queries.CountCommunityPostDislike;

public record CountCommunityPostDislikeQuery(
    int CommunityPostId
    ) : IRequest<int>;
