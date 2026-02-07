using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakenCount;

public record GetDamageTakenCountQuery(
    int CombatPlayerId
    ) : IRequest<int>;
