using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunitiesByUserId;

public record GetCommunitiesByUserIdQuery(
    Guid AppUserId,
    int Page,
    int PageSize
    ) : IRequest<AllCommunityDto>;
