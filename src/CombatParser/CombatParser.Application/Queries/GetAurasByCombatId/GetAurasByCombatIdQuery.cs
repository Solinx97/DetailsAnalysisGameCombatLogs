using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

public record GetAurasByCombatIdQuery(
    int CombatId,
    string UnitId
    ) : IRequest<IEnumerable<CombatPlayerAuraDto>>;
