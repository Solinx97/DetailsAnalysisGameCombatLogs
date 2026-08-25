using MediatR;

namespace Communication.Application.Commands.UpdateDiscussionCommentContent;

public record UpdateDiscussionCommentContentCommand(
    int Id,
    int DiscussionId,
    string Content
    ) : IRequest;
