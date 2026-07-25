using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

internal class GetResourcesGeneralsHandler(ICombatPlayerInfoRepository<ResourceRecoveryGeneral> repository, IMapper mapper) : IRequestHandler<GetResourcesGeneralsQuery, IEnumerable<ResourceRecoveryGeneralDto>>
{
    private readonly ICombatPlayerInfoRepository<ResourceRecoveryGeneral> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryGeneralDto>> Handle(GetResourcesGeneralsQuery request, CancellationToken cancellationToken)
    {
        var damageDoneGenerals = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryGeneralDto>>(damageDoneGenerals);

        return map;
    }
}
