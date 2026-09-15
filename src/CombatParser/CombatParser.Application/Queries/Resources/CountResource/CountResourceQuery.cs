using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResource;

public record CountResourceQuery(
    string UnitId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To
    ) : IRequest<int>;
