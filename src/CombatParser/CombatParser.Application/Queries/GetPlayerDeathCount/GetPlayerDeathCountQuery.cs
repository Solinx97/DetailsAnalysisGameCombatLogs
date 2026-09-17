using MediatR;

namespace CombatParser.Application.Queries.GetPlayerDeathCount;

public record GetPlayerDeathCountQuery(
    string UnitId
    ) : IRequest<int>;
