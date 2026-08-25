using MediatR;

namespace Communication.Application.Commands.UpdateCommunityDiscussionTitle;

public record UpdateCommunityDiscussionCommand(
    int Id,
    string Title,
    string Content
    ) : IRequest;
