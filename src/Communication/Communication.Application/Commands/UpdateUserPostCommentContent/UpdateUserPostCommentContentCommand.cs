using MediatR;

namespace Communication.Application.Commands.UpdateUserPostCommentContent;

public record UpdateUserPostCommentContentCommand(
    int Id,
    int UserPostId,
    string Content
    ) : IRequest;
