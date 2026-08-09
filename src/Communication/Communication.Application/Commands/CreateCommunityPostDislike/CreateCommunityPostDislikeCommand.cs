using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostDislike;

public record CreateCommunityPostDislikeCommand(
    int CommunityPostId,
    string AppUserId
    ) : IRequest;
