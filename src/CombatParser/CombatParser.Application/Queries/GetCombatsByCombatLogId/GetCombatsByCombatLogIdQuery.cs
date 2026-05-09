using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatsByCombatLogId;

public record GetCombatsByCombatLogIdQuery(
    int CombatLogId
    ) : IRequest<IEnumerable<CombatDto>>;