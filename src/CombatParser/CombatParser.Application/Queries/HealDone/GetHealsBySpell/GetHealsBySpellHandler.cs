using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsBySpell;

internal class GetHealsBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository, IMapper mapper) : IRequestHandler<GetHealsBySpellQuery, IEnumerable<HealDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneDto>> Handle(GetHealsBySpellQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetBySpellAsync(request.CombatPlayerId, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneDto>>(heals);

        return map;
    }
}
