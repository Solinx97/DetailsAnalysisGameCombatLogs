using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakenCount;

internal class GetDamageTakenCountHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<GetDamageTakenCountQuery, int>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<int> Handle(GetDamageTakenCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, cancellationToken);

        return count;
    }
}
