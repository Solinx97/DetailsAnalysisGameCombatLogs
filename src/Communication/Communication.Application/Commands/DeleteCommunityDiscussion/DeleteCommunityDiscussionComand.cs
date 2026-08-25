using MediatR;

namespace Communication.Application.Commands.DeleteCommunityDiscussion;

public record DeleteCommunityDiscussionComand(
    int Id
    ) : IRequest;
