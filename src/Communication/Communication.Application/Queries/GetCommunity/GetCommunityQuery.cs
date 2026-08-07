using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunity;

public record GetCommunityQuery(
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<CommunityDto>>;
