using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunitiesByUserId;

public record GetCommunitiesByUserIdQuery(
    string AppUserId
    ) : IRequest<IEnumerable<CommunityDto>>;
