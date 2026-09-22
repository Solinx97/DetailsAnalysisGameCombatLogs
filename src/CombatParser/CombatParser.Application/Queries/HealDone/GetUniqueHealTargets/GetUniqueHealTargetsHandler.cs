using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;

internal class GetUniqueHealTargetsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetUniqueHealTargetsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueHealTargetsQuery request, CancellationToken cancellationToken)
    {
        var targetTypes = new int[] { (int)CombatUnitType.PlayerCreature, (int)CombatUnitType.Player };
        var targets = await _repository.GetUniqueTargetsByCreatorIdAsync(request.UnitId, cancellationToken, targetTypes);

        return targets;
    }
}
