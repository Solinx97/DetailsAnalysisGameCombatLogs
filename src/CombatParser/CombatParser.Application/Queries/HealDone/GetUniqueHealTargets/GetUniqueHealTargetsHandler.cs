using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;

internal class GetUniqueHealTargetsHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<GetUniqueHealTargetsQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueHealTargetsQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetUniqueTargetsAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
