using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsByAll;

public record GetHealsByAllQuery(
    int CombatPlayerId,
    string Target,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<HealDoneDto>>;
