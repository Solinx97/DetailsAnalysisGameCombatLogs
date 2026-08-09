using MediatR;

namespace Communication.Application.Commands.DeleteUserPostDislike;

public record DeleteUserPostDislikeCommand(
    int Id,
    int UserPostId
    ) : IRequest;
