using MediatR;

namespace Communication.Application.Commands.DeleteCommunity;

public record DeleteCommunityCommand(
    int Id
    ) : IRequest;
