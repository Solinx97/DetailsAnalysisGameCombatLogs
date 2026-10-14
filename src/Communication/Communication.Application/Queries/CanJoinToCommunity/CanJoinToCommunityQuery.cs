using MediatR;

namespace Communication.Application.Queries.CanJoinToCommunity;

public record CanJoinToCommunityQuery(
    Guid AppUserId,
    int CommunityId
    ) : IRequest<bool>;
