using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetInvitesToCommunity;

public record GetInvitesToCommunityQuery(
    string AppUserId
    ) : IRequest<IEnumerable<InviteToCommunityDto>>;
