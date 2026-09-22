using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals;

internal class GetDamageGeneralsHandler(IUnitInfoRepository<DamageDoneGeneral> repository, IMapper mapper) : IRequestHandler<GetDamageGeneralsQuery, IEnumerable<DamageDoneGeneralDto>>
{
    private readonly IUnitInfoRepository<DamageDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneGeneralDto>> Handle(GetDamageGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetDamageByUnitIdAsync(request.UnitId, request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
