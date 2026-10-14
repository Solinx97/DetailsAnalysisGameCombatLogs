using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostComment;

public record CreateUserPostCommentCommand(
    int UserPostId,
    string Content,
    Guid AppUserId
    ) : IRequest<UserPostCommentDto>;
