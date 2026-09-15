using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenCreators;

internal class GetUniqueDamageTakenCreatorsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageTakenCreatorsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTakenCreatorsQuery request, CancellationToken cancellationToken)
    {
        var creatorTypes = new int[] { (int)CombatUnitType.EnemyCreature, (int)CombatUnitType.Vehicle };
        var targets = await _repository.GetCreatorNamesAsync(request.UnitId, cancellationToken, creatorTypes);

        return targets;
    }
}
