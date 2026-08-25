using MediatR;

namespace Communication.Application.Commands.AcceptInviteToCommunity;

public record AcceptInviteToCommunityCommand(
    int Id,
    int CommunityId,
    string AppUserId
    ) : IRequest;
