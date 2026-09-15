using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

internal class GetResourcesGeneralsHandler(IUnitInfoRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : IRequestHandler<GetResourcesGeneralsQuery, IEnumerable<ResourceRecoveryGeneralDto>>
{
    private readonly IUnitInfoRepository<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryGeneralDto>> Handle(GetResourcesGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetByCombatPlayerIdAsync(request.UnitId, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
