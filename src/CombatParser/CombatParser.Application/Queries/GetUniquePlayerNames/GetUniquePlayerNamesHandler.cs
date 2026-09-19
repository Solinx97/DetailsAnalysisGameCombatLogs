using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUniquePlayerNames;

internal class GetUniquePlayerNamesHandler(ICombatPlayerRepository repository) : IRequestHandler<GetUniquePlayerNamesQuery, IEnumerable<string>>
{
    private readonly ICombatPlayerRepository _repository = repository;

    public async Task<IEnumerable<string>> Handle(GetUniquePlayerNamesQuery request, CancellationToken cancellationToken)
    {
        var combatPlayerNames = await _repository.GetUniquePlayerNames(request.CombatLogId, cancellationToken);

        return combatPlayerNames;
    }
}
