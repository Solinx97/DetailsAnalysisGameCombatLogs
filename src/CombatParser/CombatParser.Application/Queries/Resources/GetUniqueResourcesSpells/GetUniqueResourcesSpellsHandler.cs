using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetUniqueResourcesSpells;

internal class GetUniqueResourcesSpellsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<GetUniqueResourcesSpellsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueResourcesSpellsQuery request, CancellationToken cancellationToken)
    {
        var creatorTypes = new int[] { (int)CombatUnitType.Player };
        var spells = await _repository.GetUniqueCreatorSpellsByTargetIdAsync(request.UnitId, cancellationToken, creatorTypes);

        return spells;
    }
}
