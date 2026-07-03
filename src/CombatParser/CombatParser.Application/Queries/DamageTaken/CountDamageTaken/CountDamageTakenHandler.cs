using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.CountDamageTaken;

internal class CountDamageTakenHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository) : IRequestHandler<CountDamageTakenQuery, int>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;

    public async Task<int> Handle(CountDamageTakenQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, cancellationToken);

        return count;
    }
}