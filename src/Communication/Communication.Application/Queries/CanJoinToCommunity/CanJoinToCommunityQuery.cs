using MediatR;

namespace Communication.Application.Queries.CanJoinToCommunity;

public record CanJoinToCommunityQuery(
    string AppUserId,
    int CommunityId
    ) : IRequest<bool>;
