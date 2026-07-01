using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResourceByAll;

public record CountResourceByAllQuery(
    int CombatPlayerId,
    string Creator,
    string Spell
    ) : IRequest<int>;
