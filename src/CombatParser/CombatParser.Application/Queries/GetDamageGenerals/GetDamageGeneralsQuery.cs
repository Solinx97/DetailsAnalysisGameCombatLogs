using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals;

public record GetDamageGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<DamageDoneGeneralDto>>;