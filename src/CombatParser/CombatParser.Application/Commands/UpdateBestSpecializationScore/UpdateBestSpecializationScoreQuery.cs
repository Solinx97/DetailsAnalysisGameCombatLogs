using MediatR;

namespace CombatParser.Application.Commands.UpdateBestSpecializationScore;

public record UpdateBestSpecializationScoreQuery(
    int Id,
    int DamageDone,
    int HealDone
    ) : IRequest;
