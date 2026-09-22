using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamage;

public record CountDamageQuery(
    string UnitId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To
    ) : IRequest<int>;
