using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostComment;

public record CreateCommunityPostCommentCommand(
    int CommunityPostId,
    string Content,
    int CommentType,
    string AppUserId
    ) : IRequest;
