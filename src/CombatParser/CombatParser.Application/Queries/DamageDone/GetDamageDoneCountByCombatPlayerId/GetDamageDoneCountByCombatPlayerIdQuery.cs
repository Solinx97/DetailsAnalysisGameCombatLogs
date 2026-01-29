using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDoneCountByCombatPlayerId;

public record GetDamageDoneCountByCombatPlayerIdQuery(
    int CombatPlayerId
    ) : IRequest<int>;
