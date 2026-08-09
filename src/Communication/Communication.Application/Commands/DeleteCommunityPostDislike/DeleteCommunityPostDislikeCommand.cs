using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostDislike;

public record DeleteCommunityPostDislikeCommand(
    int Id,
    int CommunityPostId
    ) : IRequest;
