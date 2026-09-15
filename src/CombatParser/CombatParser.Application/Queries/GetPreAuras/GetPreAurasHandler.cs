using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetPreAuras;

internal class GetPreAurasHandler(ICombatAbilityRepository repository, IMapper mapper) : IRequestHandler<GetPreAurasQuery, IEnumerable<UnitPreAuraDto>>
{
    private readonly ICombatAbilityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitPreAuraDto>> Handle(GetPreAurasQuery request, CancellationToken cancellationToken)
    {
        var combatPlayerPreAuras = !string.IsNullOrEmpty(request.UnitId)
            ? await _repository.GetByPreAuraAsync(request.CombatId, request.UnitId, cancellationToken)
            : await _repository.GetByPreAuraAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitPreAuraDto>>(combatPlayerPreAuras);

        return map;
    }
}
