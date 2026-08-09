using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionComments;

public record GetCommunityDiscussionCommentsQuery(
    int DiscussionId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommunityDiscussionCommentDto>>;
