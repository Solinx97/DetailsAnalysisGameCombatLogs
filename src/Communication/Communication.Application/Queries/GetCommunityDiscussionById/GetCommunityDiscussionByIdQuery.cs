using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionById;

public record GetCommunityDiscussionByIdQuery(
    int Id
    ) : IRequest<CommunityDiscussionDto>;
