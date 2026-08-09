using MediatR;

namespace Communication.Application.Commands.UpdateUserPostContent;

public record UpdateUserPostContentCommand(
    int Id,
    string Content
    ) : IRequest;
