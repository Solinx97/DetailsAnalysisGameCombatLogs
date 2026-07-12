using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetPotions;

public record GetPotionsQuery(
    int CombatLogId
    ) : IRequest<Dictionary<string, int>>;
