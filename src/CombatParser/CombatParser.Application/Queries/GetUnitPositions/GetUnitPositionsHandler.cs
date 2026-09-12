using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositions;

internal class GetUnitPositionsHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUnitPositionsQuery, IDictionary<string, IEnumerable<UnitPositionDto>>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitPositionDto>>> Handle(GetUnitPositionsQuery request, CancellationToken cancellationToken)
    {
        var unitsPosition = await _repository.GetPositionsAsync(request.CombatUnitId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitPositionDto>>>(unitsPosition);

        return map;
    }
}
