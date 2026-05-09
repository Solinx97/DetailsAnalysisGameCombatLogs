using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayersByCombatId;

internal class GetCombatPlayersByCombatIdHandler(ICombatPlayerRepository repository, IMapper mapper) : IRequestHandler<GetCombatPlayersByCombatIdQuery, IEnumerable<CombatPlayerDto>>
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerDto>> Handle(GetCombatPlayersByCombatIdQuery request, CancellationToken cancellationToken)
    {
        var combatPlayers = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerDto>>(combatPlayers);

        return map;
    }
}
