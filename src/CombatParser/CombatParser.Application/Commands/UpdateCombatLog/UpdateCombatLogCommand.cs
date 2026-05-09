using MediatR;

namespace CombatParser.Application.Commands.UpdateCombatLog;

public record UpdateCombatLogCommand(
    int Id,
    string Name
    ) : IRequest;
