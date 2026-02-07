using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesSpells;

public record GetUniqueResourcesSpellsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
