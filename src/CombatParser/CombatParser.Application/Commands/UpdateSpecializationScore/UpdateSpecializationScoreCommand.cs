using MediatR;

namespace CombatParser.Application.Commands.UpdateSpecializationScore;

public record UpdateSpecializationScoreCommand(
    int Id,
    double DamageScore,
    double HealScore
    ) : IRequest;
