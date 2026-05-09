using MediatR;

namespace CombatParser.Application.Commands.DeleteCombatLog;

public record DeleteCombatLogCommand(
    int Id
    ) : IRequest;
