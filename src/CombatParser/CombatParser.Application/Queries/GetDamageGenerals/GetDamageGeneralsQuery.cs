using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals;

public record GetDamageGeneralsQuery(
    string UnitId
    ) : IRequest<IEnumerable<DamageDoneGeneralDto>>;