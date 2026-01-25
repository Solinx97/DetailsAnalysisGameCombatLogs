using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetAllCombatLogs;

public record GetAllCombatLogsQuery() : IRequest<IEnumerable<CombatLogDto>>;
