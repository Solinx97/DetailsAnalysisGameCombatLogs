using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUniquCombatsByCombatLogId;

public record GetUniquCombatsByCombatLogIdQuery(
    int CombatLogId
    ) : IRequest<Dictionary<string, IEnumerable<CombatDto>>>;
