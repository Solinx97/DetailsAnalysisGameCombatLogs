using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostLike;

public record CreateCommunityPostLikeCommand(
    int CommunityPostId,
    string AppUserId
    ) : IRequest;
