using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamages;

public record GetDamagesQuery(
    int CombatPlayerId,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<DamageDoneDto>>;
