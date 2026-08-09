using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPostComments;

public record GetCommunityPostCommentsQuery(
    int CommunityPostId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommunityPostCommentDto>>;
