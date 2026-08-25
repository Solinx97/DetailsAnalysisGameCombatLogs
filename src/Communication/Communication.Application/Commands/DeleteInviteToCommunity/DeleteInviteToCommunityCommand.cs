using MediatR;

namespace Communication.Application.Commands.DeleteInviteToCommunity;

public record DeleteInviteToCommunityCommand(
    int Id,
    int CommunityId
    ) : IRequest;
