using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerDeathCount;

public record GetPlayerDeathCountQuery(
    string UnitId
    ) : IRequest<int>;
