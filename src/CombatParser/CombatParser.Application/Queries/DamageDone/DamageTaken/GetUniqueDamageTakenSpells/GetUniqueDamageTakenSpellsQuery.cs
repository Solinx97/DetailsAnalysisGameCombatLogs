using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenSpells;

public record GetUniqueDamageTakenSpellsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;
