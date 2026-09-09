using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;

public record GetDamageTakenGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<DamageDoneGeneralDto>>;