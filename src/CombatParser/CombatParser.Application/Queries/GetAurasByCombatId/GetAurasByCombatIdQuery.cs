using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

public record GetAurasByCombatIdQuery(
    int CombatId,
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerAuraDto>>;
