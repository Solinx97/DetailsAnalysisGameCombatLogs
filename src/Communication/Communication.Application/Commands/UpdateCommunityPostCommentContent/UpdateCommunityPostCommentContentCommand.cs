using MediatR;

namespace Communication.Application.Commands.UpdateCommunityPostCommentContent;

public record UpdateCommunityPostCommentContentCommand(
    int Id,
    int CommunityPostId,
    string Content
    ) : IRequest;
