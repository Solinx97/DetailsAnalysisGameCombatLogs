using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHealsByTarget;

internal class GetHealsByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> repository, IMapper mapper) : IRequestHandler<GetHealsByTargetQuery, IEnumerable<HealDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneDto>> Handle(GetHealsByTargetQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByTargetAsync(request.CombatPlayerId, request.Target, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneDto>>(heals);

        return map;
    }
}
