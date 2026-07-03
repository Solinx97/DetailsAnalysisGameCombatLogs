using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.CountHeal;

internal class CountHealHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository) : IRequestHandler<CountHealQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;

    public async Task<int> Handle(CountHealQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, cancellationToken);

        return count;
    }
}