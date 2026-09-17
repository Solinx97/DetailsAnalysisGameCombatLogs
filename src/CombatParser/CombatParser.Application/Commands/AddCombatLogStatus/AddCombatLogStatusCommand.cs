using MediatR;

namespace CombatParser.Application.Commands.AddCombatLogStatus;

public record AddCombatLogStatusCommand(
    int CombatLogId,
    int Status
    ) : IRequest;
