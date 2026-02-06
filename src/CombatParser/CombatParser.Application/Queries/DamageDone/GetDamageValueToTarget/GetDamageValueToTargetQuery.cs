using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageValueToTarget;

public record GetDamageValueToTargetQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
