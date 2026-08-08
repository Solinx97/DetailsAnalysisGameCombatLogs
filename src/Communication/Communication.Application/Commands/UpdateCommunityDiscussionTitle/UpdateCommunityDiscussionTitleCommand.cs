using MediatR;

namespace Communication.Application.Commands.UpdateCommunityDiscussionTitle;

public record UpdateCommunityDiscussionTitleCommand(
    int Id,
    string Title
    ) : IRequest;
