using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetSpecializationScore;

public record GetSpecializationScoreQuery(
    int CombatPlayerId
    ) : IRequest<SpecializationScoreDto>;
