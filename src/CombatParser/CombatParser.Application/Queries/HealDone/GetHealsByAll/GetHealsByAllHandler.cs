using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsByAll;

internal class GetHealsByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository, IMapper mapper) : IRequestHandler<GetHealsByAllQuery, IEnumerable<HealDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneDto>> Handle(GetHealsByAllQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByAllTargetsAsync(request.CombatPlayerId, request.Target, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneDto>>(heals);

        return map;
    }
}