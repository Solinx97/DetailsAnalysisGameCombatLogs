using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatsByCombatLogId;

internal class GetCombatsByCombatLogIdHandler(ICombatRepository repository, IMapper mapper) : IRequestHandler<GetCombatsByCombatLogIdQuery, IEnumerable<CombatDto>>
{
    private readonly ICombatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatDto>> Handle(GetCombatsByCombatLogIdQuery request, CancellationToken cancellationToken)
    {
        var combats = await _repository.GetByCombatLogId(request.CombatLogId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatDto>>(combats);

        return map;
    }
}

