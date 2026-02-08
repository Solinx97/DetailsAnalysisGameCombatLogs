using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetHealGenerals;

public record GetHealGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<HealDoneGeneralDto>>;