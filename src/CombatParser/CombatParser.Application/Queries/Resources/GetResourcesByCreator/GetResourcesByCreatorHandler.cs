using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesByCreator;

internal class GetResourcesByCreatorHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetResourcesByCreatorQuery, IEnumerable<ResourceRecoveryDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryDto>> Handle(GetResourcesByCreatorQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByTargetAsync(request.CombatPlayerId, request.Target, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(heals);

        return map;
    }
}
