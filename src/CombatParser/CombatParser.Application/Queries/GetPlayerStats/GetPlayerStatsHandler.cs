using AutoMapper;
using CombatParser.Application.DTOs.WoWMidnight;
using CombatParser.Application.DTOs.WoWMoPClassic;
using CombatParser.Application.Interfaces;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerStats;

internal class GetPlayerStatsHandler(ICombatPlayerRepository repository, IMapper mapper) : IRequestHandler<GetPlayerStatsQuery, IPlayerStatsDto>
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IPlayerStatsDto> Handle(GetPlayerStatsQuery request, CancellationToken cancellationToken)
    {
        var playerStats = await _repository.GetPlayerStatsAsync(request.Id, request.GameVersion, cancellationToken);
        IPlayerStatsDto map = request.GameVersion switch
        {
            0 => _mapper.Map<WoWMoPClassicPlayerStatsDto>(playerStats),
            1 => _mapper.Map<WoWMidnightPlayerStatsDto>(playerStats),
            _ => throw new ArgumentOutOfRangeException(nameof(request.GameVersion))
        };

        return map;
    }
}
