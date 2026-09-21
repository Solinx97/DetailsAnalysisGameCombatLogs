using AutoMapper;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetWhenPlayerDeath;

internal class GetWhenPlayerDeathHandler(ICombatPlayerRepository repository, IMapper mapper) : IRequestHandler<GetWhenPlayerDeathQuery, TimeSpan?>
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<TimeSpan?> Handle(GetWhenPlayerDeathQuery request, CancellationToken cancellationToken)
    {
        var whenPlayerDeath = await _repository.GetWhenPlayerDeathAsync(request.UnitId, request.SkipCount, cancellationToken);

        return whenPlayerDeath;
    }
}
