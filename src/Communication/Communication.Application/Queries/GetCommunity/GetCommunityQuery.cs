using Communication.Application.DTOs.Community.General;
using MediatR;

namespace Communication.Application.Queries.GetCommunity;

public record GetCommunityQuery(
    int Page,
    int PageSize
    ) : IRequest<AllCommunityDto>;
