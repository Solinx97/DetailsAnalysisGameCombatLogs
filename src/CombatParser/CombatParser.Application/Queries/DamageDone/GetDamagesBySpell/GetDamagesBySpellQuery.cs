using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamagesBySpell;

public record GetDamagesBySpellQuery(
    int CombatPlayerId,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageDoneDto>>;