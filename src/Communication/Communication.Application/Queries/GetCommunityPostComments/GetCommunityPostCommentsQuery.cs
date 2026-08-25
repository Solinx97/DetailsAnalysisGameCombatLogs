using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPostComments;

public record GetCommunityPostCommentsQuery(
    int CommunityPostId,
    int Page,
    int PageSize
    ) : IRequest<AllCommunityPostCommentsDto>;
