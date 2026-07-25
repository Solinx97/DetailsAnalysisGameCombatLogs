using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerCasts;

internal class GetCombatPlayerCastsHandler(ICombatPlayerInfoRepository<CombatPlayerCast> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerCastsQuery, IEnumerable<CombatPlayerCastDto>>
{
    private readonly ICombatPlayerInfoRepository<CombatPlayerCast> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerCastDto>> Handle(GetCombatPlayerCastsQuery request, CancellationToken cancellationToken)
    {
        var combatPlayerAuras = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);

        var map = _mapper.Map<IEnumerable<CombatPlayerCastDto>>(combatPlayerAuras);

        return map;
    }
}
