using MediatR;

namespace Communication.Application.Commands.CreateUserPostDislike;

public record CreateUserPostDislikeCommand(
    int UserPostId,
    string AppUserId
    ) : IRequest;