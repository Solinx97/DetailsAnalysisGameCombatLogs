using MediatR;

namespace Communication.Application.Commands.LeaveCommunityUser;

public record LeaveCommunityUserCommand(
    string AppUserId,
    int CommunityId
    ) : IRequest;
