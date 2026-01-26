using MediatR;

namespace CombatParser.Application.Commands.CombatLogIsReady;

public record CombatLogIsReadyCommand(
    int Id,
    int NumberReadyCombats, 
    int CombatsInQueue
    ) : IRequest;
