using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakens;

public record GetDamageTakensQuery(
    int CombatPlayerId,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<DamageTakenDto>>;
