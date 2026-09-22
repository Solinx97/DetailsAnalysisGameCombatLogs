using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.CountDamageTaken;

public record CountDamageTakenQuery(
    string UnitId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To
    ) : IRequest<int>;
