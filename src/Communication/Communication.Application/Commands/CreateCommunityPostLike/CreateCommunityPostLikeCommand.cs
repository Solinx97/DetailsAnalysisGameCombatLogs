using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostLike;

public record CreateCommunityPostLikeCommand(
    int CommunityId,
    int CommunityPostId,
    Guid AppUserId
    ) : IRequest<CommunityPostLikeDto>;
