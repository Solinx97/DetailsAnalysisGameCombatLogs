using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHeal;

public record CountHealQuery(
    int CombatPlayerId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To
    ) : IRequest<int>;
