using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetHealGenerals;

internal class GetHealGeneralsHandler(ICombatPlayerGenericDataRepository<HealDoneGeneral> repository, IMapper mapper) : IRequestHandler<GetHealGeneralsQuery, IEnumerable<HealDoneGeneralDto>>
{
    private readonly ICombatPlayerGenericDataRepository<HealDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneGeneralDto>> Handle(GetHealGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
