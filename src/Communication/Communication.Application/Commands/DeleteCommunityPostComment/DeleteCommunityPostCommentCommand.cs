using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostComment;

public record DeleteCommunityPostCommentCommand(
    int Id,
    int CommunityPostId
    ) : IRequest;
