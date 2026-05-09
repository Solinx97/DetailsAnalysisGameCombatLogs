using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Interfaces.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageByEachTarget;

internal class GetDamageByEachTargetHandler(IDamageFilterRepository repository, IMapper mapper) : IRequestHandler<GetDamageByEachTargetQuery, IEnumerable<IEnumerable<CombatTargetDto>>>
{
    private readonly IDamageFilterRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<IEnumerable<CombatTargetDto>>> Handle(GetDamageByEachTargetQuery request, CancellationToken cancellationToken)
    {
        var targets = await _repository.GetDamageByEachTargetAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<IEnumerable<CombatTargetDto>>>(targets);

        return map;
    }
}