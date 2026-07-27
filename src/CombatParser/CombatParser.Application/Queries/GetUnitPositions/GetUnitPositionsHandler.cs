using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositions;

internal class GetUnitPositionsHandler(IUnitPositionRepository repository, IMapper mapper) : IRequestHandler<GetUnitPositionsQuery, IDictionary<string, IEnumerable<UnitPositionDto>>>
{
    private readonly IUnitPositionRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitPositionDto>>> Handle(GetUnitPositionsQuery request, CancellationToken cancellationToken)
    {
        var unitPositions = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitPositionDto>>>(unitPositions);

        return map;
    }
}
