using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDonesByTarget;

public record GetDamageDonesByTargetQuery(
    int CombatPlayerId,
    string Target,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageDoneDto>>;