using CombatParser.Domain.Aggregates;
using MediatR;

namespace CombatParser.Application.Commands.CreateCombatLog;

public record CreateCombatLogCommand(
    string Name,
    DateTimeOffset Date,
    int LogType,
    int NumberReadyCombats,
    int CombatsInQueue,
    bool IsReady,
    string AppUserId
    ) : IRequest<CombatLog>;
