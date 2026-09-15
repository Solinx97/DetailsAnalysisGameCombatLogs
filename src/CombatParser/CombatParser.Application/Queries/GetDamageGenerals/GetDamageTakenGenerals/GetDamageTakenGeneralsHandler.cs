using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;

internal class GetDamageTakenGeneralsHandler(IUnitInfoRepository<DamageDoneGeneral> repository, IMapper mapper) : IRequestHandler<GetDamageTakenGeneralsQuery, IEnumerable<DamageDoneGeneralDto>>
{
    private readonly IUnitInfoRepository<DamageDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneGeneralDto>> Handle(GetDamageTakenGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageTakenGenerals = await _repository.GetDamageByUnitIdAsync(request.UnitId, request.CombatId, true, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(damageTakenGenerals);

        return map;
    }
}
