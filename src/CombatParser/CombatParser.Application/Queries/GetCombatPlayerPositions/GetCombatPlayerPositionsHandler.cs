using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerPositions;

internal class GetCombatPlayerPositionsHandler(ICombatPlayerPositionRepository repository, IMapper mapper) : IRequestHandler<GetCombatPlayerPositionsQuery, IEnumerable<CombatPlayerPositionDto>>
{
    private readonly ICombatPlayerPositionRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerPositionDto>> Handle(GetCombatPlayerPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerPositionDto>>(positions);

        return map;
    }
}
