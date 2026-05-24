using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

internal class GetAurasByCombatIdHandler(ICombatPlayerAuraRepository repository, IMapper mapper) : IRequestHandler<GetAurasByCombatIdQuery, IEnumerable<CombatPlayerAuraDto>>
{
    private readonly ICombatPlayerAuraRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerAuraDto>> Handle(GetAurasByCombatIdQuery request, CancellationToken cancellationToken)
    {
        var allCombatLogs = await _repository.GetByCombatIdAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerAuraDto>>(allCombatLogs);

        return map;
    }
}
