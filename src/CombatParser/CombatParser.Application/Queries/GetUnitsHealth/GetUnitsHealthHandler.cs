using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitsHealth;

internal class GetUnitsHealthHandler(ICombatDataRepository<UnitHealth> repository, IMapper mapper) : IRequestHandler<GetUnitsHealthQuery, IEnumerable<UnitHealthDto>>
{
    private readonly ICombatDataRepository<UnitHealth> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitHealthDto>> Handle(GetUnitsHealthQuery request, CancellationToken cancellationToken)
    {
        var unitsHealth = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);

        var map = _mapper.Map<IEnumerable<UnitHealthDto>>(unitsHealth);

        return map;
    }
}