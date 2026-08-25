using MediatR;

namespace Communication.Application.Commands.DeleteUserPostComment;

public record DeleteUserPostCommentCommand(
    int Id,
    int UserPostId
    ) : IRequest;
