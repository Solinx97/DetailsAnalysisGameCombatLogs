using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;

internal class GetUniqueHealSpellsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetUniqueHealSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueHealSpellsQuery request, CancellationToken cancellationToken)
    {
        var targetTypes = new int[] { (int)CombatUnitType.PlayerCreature, (int)CombatUnitType.Player };
        var spells = await _repository.GetUniqueTargetSpellsByCreatorIdAsync(request.UniId, cancellationToken, targetTypes);

        return spells;
    }
}
