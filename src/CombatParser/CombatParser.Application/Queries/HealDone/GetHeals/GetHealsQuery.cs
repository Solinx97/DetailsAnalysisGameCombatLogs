using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHeals;

public record GetHealsQuery(
    string UnitId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<HealDoneDto>>;
