using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenSpells;

internal class GetUniqueDamageTakenSpellsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageTakenSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenSpellsQuery request, CancellationToken cancellationToken)
    {
        var creatorTypes = new int[] { (int)CombatUnitType.PlayerCreature, (int)CombatUnitType.Player };
        var spells = await _repository.GetUniqueCreatorSpellsByTargetIdAsync(request.UnitId, cancellationToken, creatorTypes);

        return spells;
    }
}
