using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostLike;

public record DeleteCommunityPostLikeCommand(
    int Id,
    int CommunityPostId
    ) : IRequest;