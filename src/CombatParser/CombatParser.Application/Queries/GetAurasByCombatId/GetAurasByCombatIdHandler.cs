using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

internal class GetAurasByCombatIdHandler(ICombatPlayerAuraRepository repository, IMapper mapper) : IRequestHandler<GetAurasByCombatIdQuery, IEnumerable<UnitAuraDto>>
{
    private readonly ICombatPlayerAuraRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitAuraDto>> Handle(GetAurasByCombatIdQuery request, CancellationToken cancellationToken)
    {
        var combatAuras = await _repository.GetAurasAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitAuraDto>>(combatAuras);

        return map;
    }
}
