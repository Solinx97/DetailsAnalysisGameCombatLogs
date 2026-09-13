using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitsHealth;

internal class GetUnitsHealthHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUnitsHealthQuery, IDictionary<string, IEnumerable<UnitHealthDto>>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitHealthDto>>> Handle(GetUnitsHealthQuery request, CancellationToken cancellationToken)
    {
        var unitsHealth = await _repository.GetHealthesAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitHealthDto>>>(unitsHealth);

        return map;
    }
}