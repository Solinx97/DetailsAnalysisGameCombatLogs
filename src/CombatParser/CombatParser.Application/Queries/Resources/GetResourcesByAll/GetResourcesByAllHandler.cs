using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesByAll;

internal class GetResourcesByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetResourcesByAllQuery, IEnumerable<ResourceRecoveryDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryDto>> Handle(GetResourcesByAllQuery request, CancellationToken cancellationToken)
    {
        var resources = await _repository.GetByAllCreatorsAsync(request.CombatPlayerId, request.Creator, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(resources);

        return map;
    }
}