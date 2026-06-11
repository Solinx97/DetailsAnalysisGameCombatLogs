using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetBestSpecializationScore;

public record GetBestSpecializationScoreQuery(
    int SpecId, 
    int BossId
    ) : IRequest<BestSpecializationScoreDto>;