using MediatR;

namespace Communication.Application.Commands.DeleteDiscussionComment;

public record DeleteDiscussionCommentCommand(
    int Id,
    int DiscussionId
    ) : IRequest;
