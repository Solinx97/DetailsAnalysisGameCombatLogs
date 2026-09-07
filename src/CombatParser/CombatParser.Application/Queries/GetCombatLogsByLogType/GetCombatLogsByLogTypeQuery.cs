using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatLogsByLogType;

public record GetCombatLogsByLogTypeQuery(
    int LogType,
    int GameVersion,
    string? AppUserId
    ) : IRequest<IEnumerable<CombatLogDto>>;
