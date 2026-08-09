using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateUserPost;

public record CreateUserPostCommand(
    string Owner,
    string Content,
    int PublicType,
    string Tags,
    string AppUserId
    ) : IRequest<UserPostDto>;