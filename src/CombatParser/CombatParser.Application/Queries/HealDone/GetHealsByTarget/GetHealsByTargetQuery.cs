using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsByTarget;

public record GetHealsByTargetQuery(
    int CombatPlayerId,
    string Target,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<HealDoneDto>>;