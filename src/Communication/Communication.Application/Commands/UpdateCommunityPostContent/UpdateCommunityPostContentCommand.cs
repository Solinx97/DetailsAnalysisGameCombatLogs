using MediatR;

namespace Communication.Application.Commands.UpdateCommunityPostContent;

public record UpdateCommunityPostContentCommand(
    int Id,
    string Content
    ) : IRequest;