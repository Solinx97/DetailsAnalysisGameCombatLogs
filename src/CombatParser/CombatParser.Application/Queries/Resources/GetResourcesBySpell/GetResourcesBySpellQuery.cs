using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesBySpell;

public record GetResourcesBySpellQuery(
    int CombatPlayerId,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<ResourceRecoveryDto>>;