using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerDeaths;

internal class GetCombatPlayerDeathsHandler(ICombatPlayerDataByTimeRepository<CombatPlayerDeath> repository, IMapper mapper) : IRequestHandler<GetPlayerDeathsQuery, IEnumerable<CombatPlayerDeathDto>>
{
    private readonly ICombatPlayerDataByTimeRepository<CombatPlayerDeath> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerDeathDto>> Handle(GetPlayerDeathsQuery request, CancellationToken cancellationToken)
    {
        var playerDeaths = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerDeathDto>>(playerDeaths);

        return map;
    }
}