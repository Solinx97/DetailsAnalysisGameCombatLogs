using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageCount;

public record GetDamageCountQuery(
    int CombatPlayerId
    ) : IRequest<int>;
