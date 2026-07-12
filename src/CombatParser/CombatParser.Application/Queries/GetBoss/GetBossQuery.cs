using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetBoss;

public record GetBossQuery(
    int GameBossId,
    int Difficult, 
    int GroupSize
    ) : IRequest<BossDto>;
