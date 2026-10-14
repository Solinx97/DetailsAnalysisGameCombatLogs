using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Commands.CreateCommunity;

public record CreateCommunityCommand(
    string Name,
    string Description,
    int PolicyType,
    Guid AppUserId
    ) : IRequest<CommunityDto>;
