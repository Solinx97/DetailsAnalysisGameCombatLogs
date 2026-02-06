using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageSpells;

public record GetUniqueDamageSpellsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
