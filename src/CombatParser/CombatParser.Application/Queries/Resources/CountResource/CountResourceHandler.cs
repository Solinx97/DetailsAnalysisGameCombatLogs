using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Resources.CountResource;

internal class CountResourceHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository) : IRequestHandler<CountResourceQuery, int>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;

    public async Task<int> Handle(CountResourceQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, cancellationToken);

        return count;
    }
}
