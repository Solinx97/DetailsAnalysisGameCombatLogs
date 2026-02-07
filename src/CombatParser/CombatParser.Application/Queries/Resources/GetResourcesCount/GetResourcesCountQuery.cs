using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesCount;

public record GetResourcesCountQuery(
    int CombatPlayerId
    ) : IRequest<int>;
