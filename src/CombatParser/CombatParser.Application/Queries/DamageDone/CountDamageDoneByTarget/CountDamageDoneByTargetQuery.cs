using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamageDoneByTarget;

public record CountDamageDoneByTargetQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
