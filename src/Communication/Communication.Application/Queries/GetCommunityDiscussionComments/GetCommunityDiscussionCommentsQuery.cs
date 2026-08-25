using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionComments;

public record GetCommunityDiscussionCommentsQuery(
    int DiscussionId,
    int Page,
    int PageSize
    ) : IRequest<AllDiscussionCommentDto>;
