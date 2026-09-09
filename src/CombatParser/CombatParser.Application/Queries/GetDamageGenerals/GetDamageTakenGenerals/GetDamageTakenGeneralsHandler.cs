using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageGenerals.GetDamageTakenGenerals;

internal class GetDamageTakenGeneralsHandler(ICombatPlayerInfoRepository<DamageDoneGeneral> repository, IMapper mapper) : IRequestHandler<GetDamageTakenGeneralsQuery, IEnumerable<DamageDoneGeneralDto>>
{
    private readonly ICombatPlayerInfoRepository<DamageDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneGeneralDto>> Handle(GetDamageTakenGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetDamageByCombatPlayerIdAsync(request.CombatPlayerId, true, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
