using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetBossMap;

public record GetBossMapQuery(
    int BossMapId
    ) : IRequest<BossMapDto>;
