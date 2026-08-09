using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPost;

public record GetCommunityPostQuery(
    int CommunityId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommunityPostDto>>;