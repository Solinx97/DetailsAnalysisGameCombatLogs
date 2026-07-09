using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageSpells;

internal class GetDamageSpellsHandler(IDashboardRepository repository) : IRequestHandler<GetDamageSpellsQuery, Dictionary<string, int>>
{
    private readonly IDashboardRepository _repository = repository;

    public async Task<Dictionary<string, int>> Handle(GetDamageSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetDamageSpellsAsync(request.CombatLogId, cancellationToken);

        return spells;
    }
}