using MediatR;

namespace Communication.Application.Commands.DeleteCommunityUser;

public record DeleteCommunityUserCommand(
    string Id,
    int CommunityId
    ) : IRequest;
