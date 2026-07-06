using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenSpells;

internal class GetUniqueDamageTakenSpellsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<GetUniqueDamageTakenSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenSpellsQuery request, CancellationToken cancellationToken)
    {
        var spells = await _repository.GetUniqueSpellsAsync(request.CombatPlayerId, cancellationToken);

        return spells;
    }
}
