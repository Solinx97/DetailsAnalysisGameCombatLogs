using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussions;

public record GetCommunityDiscussionsQuery(
    int CommunityId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommunityDiscussionDto>>;
