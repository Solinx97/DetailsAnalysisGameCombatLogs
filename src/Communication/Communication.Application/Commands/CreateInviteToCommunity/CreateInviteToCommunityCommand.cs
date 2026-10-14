using MediatR;

namespace Communication.Application.Commands.CreateInviteToCommunity;

public record CreateInviteToCommunityCommand(
    int CommunityId,
    Guid AppUserId,
    Guid ToAppUserId
    ) : IRequest;
