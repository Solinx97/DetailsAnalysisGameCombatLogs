using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsBySpell;

public record GetHealsBySpellQuery(
    int CombatPlayerId,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<HealDoneDto>>;