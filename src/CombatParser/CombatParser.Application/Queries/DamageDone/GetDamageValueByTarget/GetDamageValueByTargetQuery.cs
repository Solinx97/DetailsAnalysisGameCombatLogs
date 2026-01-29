using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageValueByTarget;

public record GetDamageValueByTargetQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
