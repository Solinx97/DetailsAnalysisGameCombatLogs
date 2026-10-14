using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetInvitesToCommunity;

public record GetInvitesToCommunityQuery(
    Guid AppUserId
    ) : IRequest<IEnumerable<InviteToCommunityDto>>;
