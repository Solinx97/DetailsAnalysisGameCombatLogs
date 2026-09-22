using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;

public record GetDamageTakenGeneralsQuery(
    string UnitId,
    int CombatId
    ) : IRequest<IEnumerable<DamageDoneGeneralDto>>;