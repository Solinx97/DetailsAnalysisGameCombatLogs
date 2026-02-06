using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealByTarget;

public record CountHealByTargetQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
