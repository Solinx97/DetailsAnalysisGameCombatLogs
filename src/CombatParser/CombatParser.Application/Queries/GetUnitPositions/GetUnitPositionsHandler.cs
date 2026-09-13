using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositions;

internal class GetUnitPositionsHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUnitPositionsQuery, IEnumerable<UnitPositionDto>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitPositionDto>> Handle(GetUnitPositionsQuery request, CancellationToken cancellationToken)
    {
        var unitsPosition = await _repository.GetPositionsAsync(request.CombatUnitId, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitPositionDto>>(unitsPosition);

        return map;
    }
}
