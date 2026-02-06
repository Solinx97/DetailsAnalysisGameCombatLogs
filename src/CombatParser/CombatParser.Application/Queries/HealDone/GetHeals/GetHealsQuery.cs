using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHeals;

public record GetHealsQuery(
    int CombatPlayerId,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<HealDoneDto>>;
