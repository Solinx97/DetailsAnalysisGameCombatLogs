using MediatR;

namespace Communication.Application.Commands.CreateCommunityUser;

public record CreateCommunityUserCommand(
    int CommunityId,
    Guid AppUserId
    ) : IRequest;
