using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageTakenGenerals;

public record GetDamageTakenGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<DamageTakenGeneralDto>>;