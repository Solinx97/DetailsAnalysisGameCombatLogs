using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPreAuras;

internal class GetUnitPreAurasHandler(ICombatAbilityRepository repository, IMapper mapper) : IRequestHandler<GetUnitPreAurasQuery, IEnumerable<UnitPreAuraDto>>
{
    private readonly ICombatAbilityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitPreAuraDto>> Handle(GetUnitPreAurasQuery request, CancellationToken cancellationToken)
    {
        var unitPreAuras = await _repository.GetByPreAuraAsync(request.CombatId, request.UnitId, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitPreAuraDto>>(unitPreAuras);

        return map;
    }
}
