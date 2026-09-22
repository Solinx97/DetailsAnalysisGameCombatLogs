using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPreAuras;

public record GetUnitPreAurasQuery(
    int CombatId,
    string UnitId
    ) : IRequest<IEnumerable<UnitPreAuraDto>>;
