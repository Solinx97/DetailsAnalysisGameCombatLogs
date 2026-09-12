using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatUnits;

internal class GetCombatUnitsHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetCombatUnitsQuery, IEnumerable<CombatUnitDto>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatUnitDto>> Handle(GetCombatUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await _repository.GetAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatUnitDto>>(units);

        return map;
    }
}
