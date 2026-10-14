using MediatR;

namespace Communication.Application.Commands.DeleteCommunityUser;

public record DeleteCommunityUserCommand(
    Guid Id,
    int CommunityId
    ) : IRequest;
