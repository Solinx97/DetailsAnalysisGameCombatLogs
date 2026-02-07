using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesBySpell;

internal class GetResourcesBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetResourcesBySpellQuery, IEnumerable<ResourceRecoveryDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ResourceRecoveryDto>> Handle(GetResourcesBySpellQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetBySpellAsync(request.CombatPlayerId, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<ResourceRecoveryDto>>(heals);

        return map;
    }
}
