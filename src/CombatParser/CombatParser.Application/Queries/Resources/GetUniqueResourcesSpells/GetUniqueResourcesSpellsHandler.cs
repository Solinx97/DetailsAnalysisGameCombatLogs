using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesSpells;

internal class GetUniqueResourcesSpellsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<GetUniqueResourcesSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueResourcesSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
