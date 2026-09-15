using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetPreAuras;

public record GetPreAurasQuery(
    int CombatId,
    string UnitId
    ) : IRequest<IEnumerable<CombatPlayerPreAuraDto>>;