using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetUniquCombatsByCombatLogId;

internal class GetUniquCombatsByCombatLogIdHandler(ICombatRepository repository, IMapper mapper) : IRequestHandler<GetUniquCombatsByCombatLogIdQuery, Dictionary<string, IEnumerable<CombatDto>>>
{
    private readonly ICombatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Dictionary<string, IEnumerable<CombatDto>>> Handle(GetUniquCombatsByCombatLogIdQuery request, CancellationToken cancellationToken)
    {
        var uniqueCombats = await _repository.GetUniqueByCombatLogIdAsync(request.CombatLogId, cancellationToken);
        var map = _mapper.Map<Dictionary<string, IEnumerable<CombatDto>>>(uniqueCombats);

        return map;
    }
}
