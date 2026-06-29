using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamagesByAll;

internal class GetDamagesByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamagesByAllQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamagesByAllQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetByAllAsync(request.CombatPlayerId, request.Target, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damages);

        return map;
    }
}