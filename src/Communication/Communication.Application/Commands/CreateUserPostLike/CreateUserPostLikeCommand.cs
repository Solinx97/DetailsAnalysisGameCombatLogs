using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostLike;

public record CreateUserPostLikeCommand(
    int UserPostId,
    Guid AppUserId
    ) : IRequest<UserPostLikeDto>;
