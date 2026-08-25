using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPost;

public record GetCommunityPostQuery(
    int CommunityId,
    string AppUserId,
    int Page,
    int PageSize
    ) : IRequest<AllCommunityPostsDto>;