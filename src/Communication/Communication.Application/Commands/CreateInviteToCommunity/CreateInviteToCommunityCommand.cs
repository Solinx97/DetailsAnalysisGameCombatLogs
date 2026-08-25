using MediatR;

namespace Communication.Application.Commands.CreateInviteToCommunity;

public record CreateInviteToCommunityCommand(
    int CommunityId,
    string AppUserId,
    string ToAppUserId
    ) : IRequest;
