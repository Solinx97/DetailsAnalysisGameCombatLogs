using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetByIdCombatLog;

public record GetByIdCombatLogQuery(
    int Id
    ) : IRequest<CombatLogDto>;
