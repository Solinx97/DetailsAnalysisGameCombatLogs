using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitCasts;

internal class GetUnitCastsHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUnitCastsQuery, IDictionary<string, IEnumerable<UnitCastDto>>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IDictionary<string, IEnumerable<UnitCastDto>>> Handle(GetUnitCastsQuery request, CancellationToken cancellationToken)
    {
        var unitsCast = await _repository.GetCastsAsync(request.CombatUnitId, cancellationToken);
        var map = _mapper.Map<IDictionary<string, IEnumerable<UnitCastDto>>>(unitsCast);

        return map;
    }
}
