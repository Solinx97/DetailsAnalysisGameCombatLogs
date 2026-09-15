using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesCreators;

public record GetUniqueResourcesCreatorsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;