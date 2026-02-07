using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResources;

internal class GetResourcesHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetResourcesQuery, IEnumerable<ResourceRecoveryDto>>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryDto>> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(heals);

        return map;
    }
}

