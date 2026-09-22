using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHeal;

public record CountHealQuery(
    string UnitId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To
    ) : IRequest<int>;
