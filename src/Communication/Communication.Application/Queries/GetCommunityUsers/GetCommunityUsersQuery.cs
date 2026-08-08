using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsers;

public record GetCommunityUsersQuery(
    int CommunityId
    ) : IRequest<IEnumerable<CommunityUserDto>>;
