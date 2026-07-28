using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitsHealth;

internal class GetUnitsHealthHandler(IUnitRepository<UnitHealth> repository, IMapper mapper) : IRequestHandler<GetUnitsHealthQuery, IDictionary<string, IEnumerable<UnitHealthDto>>>
{
    private readonly IUnitRepository<UnitHealth> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitHealthDto>>> Handle(GetUnitsHealthQuery request, CancellationToken cancellationToken)
    {
        var unitsHealth = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitHealthDto>>>(unitsHealth);

        return map;
    }
}