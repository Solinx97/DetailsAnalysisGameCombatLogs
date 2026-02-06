using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageSpells;

internal class GetUniqueDamageSpellsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
