using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenByAll;

public record CountDamageTakenByAllQuery(
    int CombatPlayerId,
    string Creator,
    string Spell
    ) : IRequest<int>;
