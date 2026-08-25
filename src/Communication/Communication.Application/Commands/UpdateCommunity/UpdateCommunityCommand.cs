using MediatR;

namespace Communication.Application.Commands.UpdateCommunity;

public record UpdateCommunityCommand(
    int Id,
    string Name,
    string Description
    ) : IRequest;