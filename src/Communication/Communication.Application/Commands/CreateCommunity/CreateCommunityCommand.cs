using Communication.Domain.Aggregates;
using MediatR;

namespace Communication.Application.Commands.CreateCommunity;

public record CreateCommunityCommand(
    string Name,
    string Description,
    int PolicyType,
    string AppUserId
    ) : IRequest<Community>;
