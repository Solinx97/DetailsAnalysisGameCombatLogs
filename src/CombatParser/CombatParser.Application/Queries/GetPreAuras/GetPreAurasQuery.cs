using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetPreAuras;

public record GetPreAurasQuery(
    int CombatId,
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerPreAuraDto>>;