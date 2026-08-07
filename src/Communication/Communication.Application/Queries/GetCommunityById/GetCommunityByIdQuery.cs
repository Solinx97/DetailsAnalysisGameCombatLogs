using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Queries.GetCommunityById;

public record GetCommunityByIdQuery(
    int Id
    ) : IRequest<CommunityDto>;