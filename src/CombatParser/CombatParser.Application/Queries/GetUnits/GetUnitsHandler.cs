using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnits;

internal class GetUnitsHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUnitsQuery, IEnumerable<UnitDto>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        if (request.CombatId <= 0)
        {
            return [];
        }

        var units = await _repository.GetAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitDto>>(units);

        return map;
    }
}
