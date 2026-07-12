using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHealSpells;

internal class GetHealSpellsHandler(IDashboardRepository repository) : IRequestHandler<GetHealSpellsQuery, Dictionary<string, int>>
{
    private readonly IDashboardRepository _repository = repository;

    public async Task<Dictionary<string, int>> Handle(GetHealSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetHealSpellsAsync(request.CombatLogId, cancellationToken);

        return spells;
    }
}