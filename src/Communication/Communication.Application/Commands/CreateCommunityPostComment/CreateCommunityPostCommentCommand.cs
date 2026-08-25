using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostComment;

public record CreateCommunityPostCommentCommand(
    int CommunityId,
    int CommunityPostId,
    string Content,
    int CommentType,
    string AppUserId
    ) : IRequest<CommunityPostCommentDto>;
