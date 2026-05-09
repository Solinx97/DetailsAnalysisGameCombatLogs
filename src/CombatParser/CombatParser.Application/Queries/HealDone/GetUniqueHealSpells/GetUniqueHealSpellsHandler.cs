using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;

internal class GetUniqueHealSpellsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetUniqueHealSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueHealSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
