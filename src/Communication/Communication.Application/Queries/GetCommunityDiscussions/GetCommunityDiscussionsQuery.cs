using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussions;

public record GetCommunityDiscussionsQuery(
    int CommunityId,
    int Page,
    int PageSize
    ) : IRequest<AllDiscussionDto>;
