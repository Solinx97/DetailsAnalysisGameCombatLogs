using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostDislike;

public record CreateUserPostDislikeCommand(
    int UserPostId,
    string AppUserId
    ) : IRequest<UserPostDislikeDto>;