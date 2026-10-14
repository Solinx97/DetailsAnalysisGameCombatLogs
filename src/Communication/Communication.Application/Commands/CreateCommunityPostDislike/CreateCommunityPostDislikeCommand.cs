using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostDislike;

public record CreateCommunityPostDislikeCommand(
    int CommunityId,
    int CommunityPostId,
    Guid AppUserId
    ) : IRequest<CommunityPostDislikeDto>;
