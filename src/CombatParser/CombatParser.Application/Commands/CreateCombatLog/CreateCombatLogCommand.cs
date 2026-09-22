using CombatParser.Domain.Aggregates;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombatLog;

public record CreateCombatLogCommand(
    int GameVersion,
    string Name,
    int LogType,
    string AppUserId
    ) : IRequest<int>;
