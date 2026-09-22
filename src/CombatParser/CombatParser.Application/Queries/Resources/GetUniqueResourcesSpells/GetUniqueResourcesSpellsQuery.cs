using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesSpells;

public record GetUniqueResourcesSpellsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;
