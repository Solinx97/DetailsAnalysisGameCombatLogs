using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamagesByAll;

public record GetDamageByAllQuery(
    int CombatPlayerId,
    string Target,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<DamageDoneDto>>;
