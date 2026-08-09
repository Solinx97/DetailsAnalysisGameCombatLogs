using MediatR;

namespace Communication.Application.Commands.DeleteUserPost;

public record DeleteUserPostCommand(
    int Id
    ) : IRequest;
