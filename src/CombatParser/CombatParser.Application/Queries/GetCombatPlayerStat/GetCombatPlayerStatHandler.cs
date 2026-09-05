using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.WoWMoPClassic;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStat;

internal class GetCombatPlayerStatHandler(ICombatPlayerInfoRepository<WoWMoPClassicPlayerStats> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerStatQuery, CombatPlayerStatsDto>
{
    private readonly ICombatPlayerInfoRepository<WoWMoPClassicPlayerStats> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerStatsDto> Handle(GetCombatPlayerStatQuery request, CancellationToken cancellationToken)
    {
        var stats = await _repository.GetFirstByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<CombatPlayerStatsDto>(stats);

        return map;
    }
}
