using MediatR;

namespace Communication.Application.Commands.UpdateCommunityName;

public record UpdateCommunityNameCommand(
    int Id,
    string Name
    ) : IRequest;