using MediatR;

namespace Communication.Application.Commands.CreateUserPostLike;

public record CreateUserPostLikeCommand(
    int UserPostId,
    string AppUserId
    ) : IRequest;
