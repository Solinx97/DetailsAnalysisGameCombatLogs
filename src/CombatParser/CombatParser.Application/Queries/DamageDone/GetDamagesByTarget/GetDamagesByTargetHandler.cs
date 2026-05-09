using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamagesByTarget;

internal class GetDamagesByTargetHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamagesByTargetQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamagesByTargetQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetByTargetAsync(request.CombatPlayerId, request.Target, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damages);

        return map;
    }
}
