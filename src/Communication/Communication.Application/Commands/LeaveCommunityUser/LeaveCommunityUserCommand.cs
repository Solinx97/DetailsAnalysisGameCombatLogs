using MediatR;

namespace Communication.Application.Commands.LeaveCommunityUser;

public record LeaveCommunityUserCommand(
    Guid AppUserId,
    int CommunityId
    ) : IRequest;
