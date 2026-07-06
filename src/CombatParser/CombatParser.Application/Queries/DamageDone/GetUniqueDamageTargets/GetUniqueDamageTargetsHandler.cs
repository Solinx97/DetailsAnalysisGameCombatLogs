using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;

internal class GetUniqueDamageTargetsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueDamageTargetsQuery, IEnumerable<string>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueDamageTargetsQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetUniqueTargetsAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
