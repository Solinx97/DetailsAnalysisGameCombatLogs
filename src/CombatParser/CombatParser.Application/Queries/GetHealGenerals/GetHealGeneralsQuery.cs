using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetHealGenerals;

public record GetHealGeneralsQuery(
    string UnitId
    ) : IRequest<IEnumerable<HealDoneGeneralDto>>;