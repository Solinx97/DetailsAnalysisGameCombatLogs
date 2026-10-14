using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsersByUserId;

public record GetCommunityUsersByUserIdQuery(
    Guid AppUserId,
    int Page,
    int PageSize
    ) : IRequest<AllCommunityUserDto>;
