using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHealByAll;

public record CountHealByAllQuery(
    int CombatPlayerId,
    string Target,
    string Spell
    ) : IRequest<int>;
