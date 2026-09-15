using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

internal class GetAurasByCombatIdHandler(ICombatPlayerAuraRepository repository, IMapper mapper) : IRequestHandler<GetAurasByCombatIdQuery, IEnumerable<CombatPlayerAuraDto>>
{
    private readonly ICombatPlayerAuraRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerAuraDto>> Handle(GetAurasByCombatIdQuery request, CancellationToken cancellationToken)
    {
        var combatPlayerAuras = !string.IsNullOrEmpty(request.UnitId)
            ? await _repository.GetAurasAsync(request.UnitId, cancellationToken)
            : await _repository.GetAurasAsync(request.CombatId, cancellationToken);

        var map = _mapper.Map<IEnumerable<CombatPlayerAuraDto>>(combatPlayerAuras);

        return map;
    }
}
