using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDonesByCombatPlayerId;

public record GetDamageDonesByCombatPlayerIdQuery(
    int CombatPlayerId,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<DamageDoneDto>>;
