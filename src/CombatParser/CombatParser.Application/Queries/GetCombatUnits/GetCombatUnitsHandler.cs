using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatUnits;

internal class GetCombatUnitsHandler(ICombatDataRepository<CombatUnit> repository, IMapper mapper) : IRequestHandler<GetCombatUnitsQuery, IEnumerable<CombatUnitDto>>
{
    private readonly ICombatDataRepository<CombatUnit> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatUnitDto>> Handle(GetCombatUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatUnitDto>>(units);

        return map;
    }
}
