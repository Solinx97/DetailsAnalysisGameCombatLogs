using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageTakenGenerals;

public record GetDamageTakenGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<DamageTakenGeneralDto>>;