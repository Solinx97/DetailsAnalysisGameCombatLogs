using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamages;

internal class GetDamagesHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamagesQuery, IEnumerable<DamageDoneDto>>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamagesQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damages);

        return map;
    }
}

