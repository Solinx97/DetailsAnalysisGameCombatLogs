using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerPositions;

public record GetCombatPlayerPositionsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerPositionDto>>;
