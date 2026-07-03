using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.CountDamage;

internal class CountDamageHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository) : IRequestHandler<CountDamageQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;

    public async Task<int> Handle(CountDamageQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, cancellationToken);

        return count;
    }
}