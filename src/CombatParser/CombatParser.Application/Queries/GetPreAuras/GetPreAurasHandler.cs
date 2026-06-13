using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetPreAuras;

internal class GetPreAurasHandler(ICombatAbilityRepository repository, IMapper mapper) : IRequestHandler<GetPreAurasQuery, IEnumerable<CombatPlayerPreAuraDto>>
{
    private readonly ICombatAbilityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatPlayerPreAuraDto>> Handle(GetPreAurasQuery request, CancellationToken cancellationToken)
    {
        var preAuras = await _repository.GetByPreAuraAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatPlayerPreAuraDto>>(preAuras);

        return map;
    }
}
