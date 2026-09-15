using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetHealGenerals;

internal class GetHealGeneralsHandler(IUnitInfoRepository<HealDoneGeneral> repository, IMapper mapper) : IRequestHandler<GetHealGeneralsQuery, IEnumerable<HealDoneGeneralDto>>
{
    private readonly IUnitInfoRepository<HealDoneGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneGeneralDto>> Handle(GetHealGeneralsQuery request, CancellationToken cancellationToken)
    {
        var healDoneGenerals = await _repository.GetHealByUnitIdAsync(request.UnitId, request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneGeneralDto>>(healDoneGenerals);

        return map;
    }
}
