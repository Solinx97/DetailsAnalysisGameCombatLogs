using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenSpells;

internal class GetUniqueDamageTakenSpellsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<GetUniqueDamageTakenSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
