using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealCount;

public record GetHealCountQuery(
    int CombatPlayerId
    ) : IRequest<int>;
