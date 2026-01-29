using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDonesBySpell;

public record GetDamageDonesBySpellQuery(
    int CombatPlayerId,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageDoneDto>>;