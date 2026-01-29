using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageDonesByCombatPlayerId;

internal class GetDamageDonesByCombatPlayerIdHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamageDonesByCombatPlayerIdQuery, IEnumerable<DamageDoneDto>>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamageDonesByCombatPlayerIdQuery request, CancellationToken cancellationToken)
    {
        var damageDones = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damageDones);

        return map;
    }
}

