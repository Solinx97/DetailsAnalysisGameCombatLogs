using CombatParser.Domain.Aggregates;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombatLog;

public record CreateCombatLogCommand(
    string Name,
    int LogType,
    string AppUserId
    ) : IRequest<CombatLog>;
