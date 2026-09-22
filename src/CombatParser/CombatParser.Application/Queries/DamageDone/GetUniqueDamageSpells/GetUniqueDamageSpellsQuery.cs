using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageSpells;

public record GetUniqueDamageSpellsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;
