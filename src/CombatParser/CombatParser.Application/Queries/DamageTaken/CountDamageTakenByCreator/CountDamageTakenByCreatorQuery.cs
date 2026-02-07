using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTakenByCreator;

public record CountDamageTakenByCreatorQuery(
    int CombatPlayerId,
    string Target
    ) : IRequest<int>;
