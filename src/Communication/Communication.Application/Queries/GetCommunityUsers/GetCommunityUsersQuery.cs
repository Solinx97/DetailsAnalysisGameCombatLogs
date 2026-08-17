using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsers;

public record GetCommunityUsersQuery(
    int CommunityId,
    int Page,
    int PageSize
    ) : IRequest<AllCommunityUserDto>;
