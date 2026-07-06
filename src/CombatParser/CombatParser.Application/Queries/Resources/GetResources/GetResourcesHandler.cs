using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResources;

internal class GetResourcesHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetResourcesQuery, IEnumerable<ResourceRecoveryDto>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryDto>> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.GetAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(resources);

        return map;
    }
}

