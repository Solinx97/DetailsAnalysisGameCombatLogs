using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerDeathCount;

internal class GetPlayerDeathCountHandler(ICombatPlayerRepository repository) : IRequestHandler<GetPlayerDeathCountQuery, int>
{
    private readonly ICombatPlayerRepository _repository = repository;

    public async Task<int> Handle(GetPlayerDeathCountQuery request, CancellationToken cancellationToken)
    {
        var playerDeathCount = await _repository.GetPlayerDeathCountAsync(request.UnitId, cancellationToken);

        return playerDeathCount;
    }
}
