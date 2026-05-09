using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetDamageTakenGenerals;

internal class GetDamageTakenGeneralsHandler(ICombatPlayerGenericDataRepository<DamageTakenGeneral> repository, IMapper mapper) : IRequestHandler<GetDamageTakenGeneralsQuery, IEnumerable<DamageTakenGeneralDto>>
{
    private readonly ICombatPlayerGenericDataRepository<DamageTakenGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenGeneralDto>> Handle(GetDamageTakenGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
