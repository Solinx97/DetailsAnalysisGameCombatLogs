using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamagesByTarget;

public record GetDamagesByTargetQuery(
    int CombatPlayerId,
    string Target,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageDoneDto>>;