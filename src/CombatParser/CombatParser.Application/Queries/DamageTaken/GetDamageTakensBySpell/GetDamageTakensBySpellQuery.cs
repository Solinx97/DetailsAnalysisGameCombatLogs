using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensBySpell;

public record GetDamageTakensBySpellQuery(
    int CombatPlayerId,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageTakenDto>>;