using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamages;

internal class GetDamagesHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamagesQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamagesQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damages);

        return map;
    }
}

