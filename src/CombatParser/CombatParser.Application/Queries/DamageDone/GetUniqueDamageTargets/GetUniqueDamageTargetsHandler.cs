using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;

internal class GetUniqueDamageTargetsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageTargetsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTargetsQuery request, CancellationToken cancellationToken)
    {
        var targetTypes = new int[] { (int)CombatUnitType.EnemyCreature, (int)CombatUnitType.Vehicle };
        var targets = await _repository.GetUniqueTargetsAsync(request.UnitId, cancellationToken, targetTypes);

        return targets;
    }
}
