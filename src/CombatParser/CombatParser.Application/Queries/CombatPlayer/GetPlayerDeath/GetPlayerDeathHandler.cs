using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerDeath;

internal class GetPlayerDeathHandler(ICombatPlayerRepository repository, IMapper mapper) : IRequestHandler<GetPlayerDeathQuery, IEnumerable<CombatPlayerDeathDto>>
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerDeathDto>> Handle(GetPlayerDeathQuery request, CancellationToken cancellationToken)
    {
        var playerDeath = await _repository.GetPlayerDeathAsync(request.UnitId, request.WhenDied, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerDeathDto>>(playerDeath);

        return map;
    }
}
