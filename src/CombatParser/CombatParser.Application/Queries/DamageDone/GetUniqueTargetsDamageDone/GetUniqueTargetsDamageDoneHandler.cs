using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueTargetsDamageDone;

internal class GetUniqueTargetsDamageDoneHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<GetUniqueTargetsDamageDoneQuery, IEnumerable<string>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniqueTargetsDamageDoneQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetUniqueTargetsByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);

        return targets;
    }
}
