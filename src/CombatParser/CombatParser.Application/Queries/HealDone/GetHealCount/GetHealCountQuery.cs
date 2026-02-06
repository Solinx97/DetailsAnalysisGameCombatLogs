using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealCountByCombatPlayerId;

public record GetHealCountQuery(
    int CombatPlayerId
    ) : IRequest<int>;
