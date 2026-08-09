using MediatR;

namespace Communication.Application.Commands.DeleteUserPostLike;

public record DeleteUserPostLikeCommand(
    int Id,
    int UserPostId
    ) : IRequest;
