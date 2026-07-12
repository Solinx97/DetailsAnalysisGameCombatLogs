using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetPotions;

internal class GetPotionsHandler(ICombatAbilityRepository repository) : IRequestHandler<GetPotionsQuery, Dictionary<string, int>>
{
    private readonly ICombatAbilityRepository _repository = repository;

    public async Task<Dictionary<string, int>> Handle(GetPotionsQuery request, CancellationToken cancellationToken)
    {
        var potions = await _repository.GetPotionsAsync(request.CombatLogId, cancellationToken);

        return potions;
    }
}
