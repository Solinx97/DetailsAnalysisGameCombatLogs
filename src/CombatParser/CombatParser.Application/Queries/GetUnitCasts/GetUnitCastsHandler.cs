using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitCasts;

internal class GetUnitCastsHandler(IUnitRepository<UnitCast> repository, IMapper mapper) : IRequestHandler<GetUnitCastsQuery, IDictionary<string, IEnumerable<UnitCastDto>>>
{
    private readonly IUnitRepository<UnitCast> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitCastDto>>> Handle(GetUnitCastsQuery request, CancellationToken cancellationToken)
    {
        var unitsCast = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitCastDto>>>(unitsCast);

        return map;
    }
}
