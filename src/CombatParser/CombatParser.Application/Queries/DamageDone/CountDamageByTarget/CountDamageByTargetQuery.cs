using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageByTarget;

public record CountDamageByTargetQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
