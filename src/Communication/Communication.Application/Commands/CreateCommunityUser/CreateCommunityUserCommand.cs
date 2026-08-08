using MediatR;

namespace Communication.Application.Commands.CreateCommunityUser;

public record CreateCommunityUserCommand(
    int CommunityId,
    string AppUserId
    ) : IRequest;
