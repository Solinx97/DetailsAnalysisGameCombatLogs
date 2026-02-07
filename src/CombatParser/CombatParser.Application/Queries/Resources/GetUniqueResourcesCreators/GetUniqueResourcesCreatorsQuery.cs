using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesCreators;

public record GetUniqueResourcesCreatorsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;